using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Data;
using SpyFallBackend.Models;

namespace SpyFallBackend.Services
{
    public class GameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Create a game table without a word list
        public async Task<GameTable> CreateGameTable(int playerCount)
        {
            var gameTable = new GameTable
            {
                PlayerCount = playerCount,
                TableKey = GenerateRandomTableKey(),
                GameStatus = "Created"
            };

            _context.GameTables.Add(gameTable);
            await _context.SaveChangesAsync();
            return gameTable;
        }

        // 2. Assign word list and start the game
        public async Task<bool> StartGame(Guid gameTableId, Guid wordListId)
        {
            var gameTable = await GetGameTableWithPlayers(gameTableId);
            if (gameTable == null) return false;

            if (gameTable.GameStatus == "Started")
            {
                throw new InvalidOperationException("The game has already started.");
            }

            // Check if the word list exists
            var wordList = await _context.WordLists.Include(wl => wl.Words).FirstOrDefaultAsync(wl => wl.WordListId == wordListId);
            if (wordList == null)
            {
                throw new InvalidOperationException("Invalid Word List ID provided.");
            }

            // Assign the word list to the game table
            gameTable.WordList = wordList;

            // Set game status to started
            gameTable.GameStatus = "Started";

            // Assign spy and select word as usual
            var spyAssigned = await AssignRandomSpy(gameTableId);
            if (!spyAssigned)
            {
                throw new InvalidOperationException("Failed to assign a spy. Make sure there are players in the game.");
            }

            var wordSelected = await SelectRandomWord(gameTableId);
            if (!wordSelected)
            {
                throw new InvalidOperationException("Failed to select a word. Make sure there is a word list associated with the game.");
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Method to assign a random player as the spy
        private async Task<bool> AssignRandomSpy(Guid gameTableId)
        {
            var gameTable = await _context.GameTables.Include(gt => gt.Players).FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
            if (gameTable == null || gameTable.Players == null || !gameTable.Players.Any())
            {
                return false;
            }

            // Reset all players' spy status to false
            foreach (var player in gameTable.Players)
            {
                player.IsSpy = false;
            }

            // Randomly select a player to be the spy
            var random = new Random();
            int spyIndex = random.Next(gameTable.Players.Count);
            var selectedSpy = gameTable.Players.ElementAt(spyIndex);
            selectedSpy.IsSpy = true;

            await _context.SaveChangesAsync();
            return await _context.Players.AnyAsync(p => p.GameTableId == gameTableId && p.IsSpy);
        }

        // Method to select a random word from the word list associated with the game table
        private async Task<bool> SelectRandomWord(Guid gameTableId)
        {
            var gameTable = await _context.GameTables
                .Include(gt => gt.WordList)
                .ThenInclude(wl => wl.Words)
                .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);

            if (gameTable?.WordList?.Words == null || !gameTable.WordList.Words.Any())
            {
                Console.WriteLine($"No words found in the word list for game table ID {gameTableId}.");
                return false;
            }

            // Select a random word from the associated word list
            var random = new Random();
            int wordIndex = random.Next(gameTable.WordList.Words.Count);
            var selectedWord = gameTable.WordList.Words.ElementAt(wordIndex);

            // Store the selected word in the GameTable object
            gameTable.SelectedWord = selectedWord.WordText; // Add a new property 'SelectedWord' in GameTable model

            await _context.SaveChangesAsync();
            Console.WriteLine($"Selected word for this round: {selectedWord.WordText}");

            return true;
        }


        // 3. Method to end the round and start a new one
        public async Task<bool> EndRound(Guid gameTableId)
        {
            var gameTable = await GetGameTableWithPlayers(gameTableId);
            if (gameTable == null) return false;

            // Increment the current round number
            gameTable.CurrentRound += 1;

            // Assign a new spy and select a new word for the next round
            var spyAssigned = await AssignRandomSpy(gameTableId);
            if (!spyAssigned)
            {
                throw new InvalidOperationException("Failed to assign a new spy for the next round.");
            }

            var wordSelected = await SelectRandomWord(gameTableId);
            if (!wordSelected)
            {
                throw new InvalidOperationException("Failed to select a new word for the next round.");
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Method to get the status of the game table
        public async Task<GameTable?> GetGameStatus(Guid gameTableId)
        {
            return await _context.GameTables
                .Include(gt => gt.Players)
                .Include(gt => gt.WordList)
                .ThenInclude(wl => wl.Words)
                .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
        }

        // Helper method to get game table with players included
        private async Task<GameTable?> GetGameTableWithPlayers(Guid gameTableId)
        {
            return await _context.GameTables
                .Include(gt => gt.Players)
                .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
        }

        // Helper method to generate a random table key
        private string GenerateRandomTableKey()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<string?> OpenBox(Guid gameTableId, Guid playerId)
        {
            // Retrieve the game table and the players
            var gameTable = await _context.GameTables
                .Include(gt => gt.Players)
                .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);

            if (gameTable == null)
            {
                Console.WriteLine($"Game table with ID {gameTableId} not found.");
                return null;
            }

            // Find the player in the game table
            var player = gameTable.Players.FirstOrDefault(p => p.PlayerId == playerId);
            if (player == null)
            {
                Console.WriteLine($"Player with ID {playerId} not found.");
                return null;
            }

            // Check if the player is a spy
            if (player.IsSpy)
            {
                return "You are the Spy! Try to blend in and figure out the word based on the discussion.";
            }

            // Return the selected word for non-spy players
            return gameTable.SelectedWord;
        }

    }

}
