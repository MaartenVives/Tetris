﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teris_V2
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for(int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    
                    if(!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }
        public GameGrid GameGrid { get; }
        public BlockQueue Blockqueue { get; }

        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }


        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            Blockqueue = new BlockQueue();
            currentBlock = Blockqueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach(Position p in currentBlock.TilePositions() )
            {
                if(!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock()
        {
            if(!CanHold)
            {
                return;
            }

            if(HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                currentBlock = Blockqueue.GetAndUpdate();
            }
            else
            {
                Block tmp = currentBlock;
                currentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }
        public void RotateBlockCW()
        {
            currentBlock.RotateCW();

            if(!BlockFits())
            {
                currentBlock.RotateCCW();
            }
        }
        public void RotateBlockCCW()
        {
            currentBlock.RotateCCW();

            if (!BlockFits())
            {
                currentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            currentBlock.Move(0, -1);
            if (!BlockFits())
            {
                currentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            currentBlock.Move(0, 1);
            if (!BlockFits())
            {
                currentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEMpty(0) && GameGrid.IsRowEMpty(1));
        }
        
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if(IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = Blockqueue.GetAndUpdate();
                CanHold = true;
            }
        }
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            return drop;
        }
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}