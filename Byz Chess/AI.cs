using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Byz_Chess.Pieces;
using Byz_Chess.Resources;
using System.Windows.Documents;

namespace Byz_Chess
{
    public class AI
    {
        /// <summary>
        /// The "evaluate" depth of minimax algorithm
        /// </summary>
        private int depth;
        private int playerNr;
        private int AiNr;

        /// <summary>
        /// Piece value
        /// </summary>
        private const int pawnValue = 100;
        private const int knightValue = 320;
        private const int bishopValue = 330;
        private const int rookValue = 500;
        private const int queenValue = 900;
        private const int kingValue = 20000;

        /// <summary>
        /// Position point
        /// It didn't work as well as I expected
        /// source : https://www.chessprogramming.org/Simplified_Evaluation_Function
        /// </summary>
        private static readonly int[] bestPawnPositions = {
              0,  0,  0,  0,  0,  0,  0,  0,
             50, 50, 50, 50, 50, 50, 50, 50,
             10, 10, 20, 30, 30, 20, 10, 10,
              5,  5, 10, 25, 25, 10,  5,  5,
              0,  0,  0, 20, 20,  0,  0,  0,
              5, -5,-10,  0,  0,-10, -5,  5,
              5, 10, 10,-20,-20, 10, 10,  5,
              0,  0,  0,  0,  0,  0,  0,  0
        };

        private static readonly int[] bestKnightPositions = {
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50,
        };

        private static readonly int[] bestBishopPositions = {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20,
        };

        private static readonly int[] bestRookPositions = {
              0,  0,  0,  0,  0,  0,  0,  0,
              5, 10, 10, 10, 10, 10, 10,  5,
             -5,  0,  0,  0,  0,  0,  0, -5,
             -5,  0,  0,  0,  0,  0,  0, -5,
             -5,  0,  0,  0,  0,  0,  0, -5,
             -5,  0,  0,  0,  0,  0,  0, -5,
             -5,  0,  0,  0,  0,  0,  0, -5,
              0,  0,  0,  5,  5,  0,  0,  0
        };

        private static readonly int[] bestQueenPositions = {
             -20,-10,-10, -5, -5,-10,-10,-20,
             -10,  0,  0,  0,  0,  0,  0,-10,
             -10,  0,  5,  5,  5,  5,  0,-10,
              -5,  0,  5,  5,  5,  5,  0, -5,
               0,  0,  5,  5,  5,  5,  0, -5,
             -10,  5,  5,  5,  5,  5,  0,-10,
             -10,  0,  5,  0,  0,  0,  0,-10,
             -20,-10,-10, -5, -5,-10,-10,-20
        };

        private static readonly int[] bestKingPositions = {
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
             20, 20,  0,  0,  0,  0, 20, 20,
             20, 30, 10,  0,  0, 10, 30, 20
        };

        public AI(int _depth, int player)
        {
            depth = _depth;
            playerNr = player;
            AiNr = playerNr ^ 3;
        }

        /// <summary>
        /// Calculate the point for evaluate
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public int CalculatePoint(ChessBoard board)
        {
            int scoreWhite = 0;
            int scoreBlack = 0;
            scoreWhite += GetScoreFromExistingPieces(playerNr, board);
            scoreBlack += GetScoreFromExistingPieces(AiNr, board);

            int evaluation = scoreBlack - scoreWhite;

            int prespective = (board.PlayerToPlay == playerNr) ? -1 : 1;
            return evaluation * prespective;
        }

        /// <summary>
        /// Get score from the existing pieces of the faction
        /// </summary>
        /// <param name="player"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        private static int GetScoreFromExistingPieces(int player, Byz_Chess.ChessBoard board)
        {
            int material = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 16; j++)
                    if (board.Positions[i][j].Piece != null)
                    {
                        if ((board.Positions[i][j].Piece is Pawn || board.Positions[i][j].Piece is InversePawn) && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (pawnValue + bestPawnPositions[i]); // plus "+ bestPawnPositions[i]" if you want, but it doesn't work well
                        }
                        if (board.Positions[i][j].Piece is Horse && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (knightValue); // plus "+ bestKnightPositions[i]" if you want, but it doesn't work well
                        }
                        if (board.Positions[i][j].Piece is Elephant && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (bishopValue); // plus "+ bestBishopPositions[i]" if you want, but it doesn't work well
                        }
                        if (board.Positions[i][j].Piece is Rook && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (rookValue); // plus "+ bestRookPositions[i]" if you want, but it doesn't work well
                        }
                        if (board.Positions[i][j].Piece is Minister && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (queenValue); // plus "+ bestQueenPositions[i]" if you want, but it doesn't work well
                        }
                        if (board.Positions[i][j].Piece is King && board.Positions[i][j].Piece?.Team == player)
                        {
                            material += (kingValue); // plus "+ bestKingPositions[i]" if you want, but it doesn't work well
                        }
                    }
            }
            return material;
        }






        //+++++++++++++++++++++++++++++++++++++ MINIMAX ALGORITHM ++++++++++++++++++++++++++++++++++++
        // if not using the lib from Stack Overflow, the code will look like below, and maybe something is missing
        //private Board GenerateMovedBoard(Board oldBoard, Move move)
        //{
        //    Board newBoard = new Board();
        //    if (oldBoard.Turn == Player.Black)
        //    {
        //        newBoard.Turn = Player.White;
        //    }
        //    else
        //    {
        //        newBoard.Turn = Player.Black;
        //    }

        //    for (int i = 0; i < 64; i++)
        //    {

        //        if (oldBoard.Pieces[i] == null)
        //        {
        //            newBoard.Pieces[i] = null;
        //        }
        //        else
        //        {
        //            if (oldBoard.Pieces[i].Player == Player.White)
        //            {
        //                if (oldBoard.Pieces[i].GetType() == typeof(Pawn))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.wPawn, Player.White, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Knight))
        //                {
        //                    newBoard.Pieces[i] = new Knight(Type.wKnight, Player.White, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Bishop))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.wBishop, Player.White, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Rook))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.wRook, Player.White, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Queen))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.wQueen, Player.White, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(King))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.wKing, Player.White, i);
        //                }
        //            }
        //            else if (oldBoard.Pieces[i].Player == Player.Black)
        //            {
        //                if (oldBoard.Pieces[i].GetType() == typeof(Pawn))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.bPawn, Player.Black, i); ;
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Knight))
        //                {
        //                    newBoard.Pieces[i] = new Knight(Type.bKnight, Player.Black, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Bishop))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.bBishop, Player.Black, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Rook))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.bRook, Player.Black, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(Queen))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.bQueen, Player.Black, i);
        //                }
        //                else if (oldBoard.Pieces[i].GetType() == typeof(King))
        //                {
        //                    newBoard.Pieces[i] = new Pawn(Type.bKing, Player.Black, i);
        //                }
        //            }
        //        }

        //    }

        //    Board.MovePiece(newBoard, move.Tile, move.Next);
        //    return newBoard;
        //}


        /// <summary>
        /// Get the piece value
        /// </summary>
        /// <param name="board"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetPieceValue(Position position)
        {
            if (position.Piece is Pawn || position.Piece is InversePawn)
            {
                return pawnValue;
            }
            else if (position.Piece is Rook)
            {
                return rookValue;
            }
            else if (position.Piece is Horse)
            {
                return knightValue;
            }
            else if (position.Piece is Elephant)
            {
                return bishopValue;
            }
            else if (position.Piece is Minister)
            {
                return queenValue;
            }
            else if (position.Piece is King)
            {
                return kingValue;
            }

            return 0;
        }

        /// <summary>
        /// Sort the list to reduce the runtime of the algorithm
        /// </summary>
        /// <param name="moveList"></param>
        /// <param name="board"></param>
        private void OrderMoves(List<Move> moveList, ChessBoard board)
        {
            int[] moveScore = new int[moveList.Count];

            for (int i = 0; i < moveList.Count; i++)
            {
                moveScore[i] = 0;

                if (moveList[i].NewPosition != null)
                {
                    moveScore[i] += 10 * GetPieceValue(moveList[i].NewPosition) - GetPieceValue(moveList[i].OldPosition);
                }
            }

            for (int sorted = 0; sorted < moveList.Count; sorted++)
            {
                int bestScore = int.MinValue;
                int bestScoreIndex = 0;

                for (int i = sorted; i < moveList.Count; i++)
                {
                    if (moveScore[i] > bestScore)
                    {
                        bestScore = moveScore[i];
                        bestScoreIndex = i;
                    }
                }

                // swap

                Move bestMove = moveList[bestScoreIndex];
                moveList[bestScoreIndex] = moveList[sorted];
                moveList[sorted] = bestMove;
            }
        }

        /// <summary>
        /// Main algorithm: minimax
        /// </summary>
        /// <param name="board"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="isMaximizingPlayer"></param>
        /// <returns></returns>
        private int Minimax(ChessBoard board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            if (depth == 0)
                return CalculatePoint(board);

            if (isMaximizingPlayer)
            {
                int bestValue = int.MinValue;

                List<Move> possibleMoves = board.GetAllLegalMoves(AiNr, board);

                OrderMoves(possibleMoves, board);
                foreach (var move in possibleMoves)
                {
                    var tempNewPositionPiece = move.NewPosition.Piece;
                    board.DoMove(move.OldPosition, move.NewPosition);

                    int value = Minimax(board, depth - 1, alpha, beta, false);

                    board.DoMove(move.NewPosition, move.OldPosition);
                    move.NewPosition.Piece = tempNewPositionPiece;

                    bestValue = Math.Max(value, bestValue);

                    alpha = Math.Max(alpha, value);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;

                List<Move> possibleMoves = board.GetAllLegalMoves(playerNr, board);

                OrderMoves(possibleMoves, board);
                foreach (var move in possibleMoves)
                {
                    var tempNewPositionPiece = move.NewPosition.Piece;
                    board.DoMove(move.OldPosition, move.NewPosition);

                    int value = Minimax(board, depth - 1, alpha, beta, true);

                    board.DoMove(move.NewPosition, move.OldPosition);
                    move.NewPosition.Piece = tempNewPositionPiece;
                    bestValue = Math.Min(value, bestValue);

                    beta = Math.Min(beta, value);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return bestValue;
            }
        }

        /// <summary>
        /// Get the result after evaluate using minimax
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Move GetBestMove(ChessBoard board)
        {
            int bestValue = int.MinValue;
            Move bestMove = new Move();
            bool turn;
            if (board.PlayerToPlay == AiNr)
            {
                turn = false;
            }
            else
            {
                turn = true;
            }

            List<Move> possibleMoves = board.GetAllLegalMoves(board.PlayerToPlay, board);

            OrderMoves(possibleMoves, board);
            foreach (var move in possibleMoves)
            {
                var tempNewPositionPiece = move.NewPosition.Piece;
                board.DoMove(move.OldPosition, move.NewPosition);

                int value = Minimax(board, depth, int.MinValue, int.MaxValue, turn);

                board.DoMove(move.NewPosition, move.OldPosition);
                move.NewPosition.Piece = tempNewPositionPiece;

                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }

            return bestMove;
        }
        //+++++++++++++ END +++++++++++++++++++ MINIMAX ALGORITHM ++++++++++++++++++++++++++++++++++++               




        //+++++++++++++++++++++++++++++++++++++ RANDOM ALGORITHM +++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// Random a move in legal moves
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Move RandomMove(ChessBoard board)
        {
            Random rand = new Random();
            List<Move> legalMoves = board.GetAllLegalMoves(AiNr, board);

            if (board.IsCheckMate() == true) return new Move();
            return legalMoves[rand.Next(0, legalMoves.Count)];
        }
        //+++++++++++++++++ END +++++++++++++++ RANDOM ALGORITHM +++++++++++++++++++++++++++++++++++++                     





        //===================================== USING EVALUATE =======================================

        /// <summary>
        /// AI generates random movement
        /// </summary>
        /// <param name="board"></param>
        public void EvaluateRandom(ChessBoard board)
        {

            Move move = RandomMove(board);

            if (move.OldPosition.Piece == null) return;

            board.DoMove(move.OldPosition, move.NewPosition);
        }

        /// <summary>
        /// AI generates evaluated movement
        /// </summary>
        /// <param name="board"></param>
        public void EvaluateAI(ChessBoard board)
        {
            // uncomment this to use negamax algorithm

            //Move move = CalculateBestMove(board, 3).Move;
            //if (move == null) return;
            //Board.MovePiece(board, move.Tile, move.Next);
            //board.Save(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\book\learn.txt", move.Tile, move.Next, board);


            // uncomment this to use minimax algorithm
            // This doesn't work as I expected, as you increase the depth (Board.cs line 286)
            // The AI will take too long to make a "stupid" movement =(((

            Move move = GetBestMove(board);
            if (move.OldPosition.Piece == null) return;
            board.DoMove(move.OldPosition, move.NewPosition);

        }

        public enum Player
        {
            White = 1,
            Black = 2
        }
    }
}