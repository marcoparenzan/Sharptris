using System;

namespace Sharptris.Models
{
    public class Stage
    {
        (int x, int y)[][][] draw = new (int x, int y)[][][]
        {
            new (int x, int y)[][] { // nothing
            },
            new (int x, int y)[][] { // cube
                new (int x, int y)[] { (-0, -0),(-0, -1),(-1, -1),(-1, -0) }
            },
            new (int x, int y)[][] { // line
                new (int x, int y)[] { (-0, -0),(-0, -1),(-0, -2),(-0, -3) },
                new (int x, int y)[] { (-1, -0),(-0, -0),(+1, -0),(+2, -0) }
            },
            new (int x, int y)[][] { // Ll
                new (int x, int y)[] { (-0, -0),(-0, -1),(-0, -2),(-1, -2) },
                new (int x, int y)[] { (-1, -0),(-1, -1),(-0, -1),(+1, -1) },
                new (int x, int y)[] { (-0, -0),(-0, -1),(-0, -2),(+1, -0) },
                new (int x, int y)[] { (-1, -0),(-0, -0),(+1, -0),(+1, -1) },
            },
            new (int x, int y)[][] { // Lr
                new (int x, int y)[] { (-0, -0),(-0, -1),(-0, -2),(+1, -2) },
                new (int x, int y)[] { (-1, -0),(-1, -1),(-0, -0),(+1, -0) },
                new (int x, int y)[] { (-0, -0),(+1, -0),(+1, -1),(+1, -2) },
                new (int x, int y)[] { (-1, -1),(-0, -1),(+1, -1),(+1, -0) },
            },
            new (int x, int y)[][] { // Sl
                new (int x, int y)[] { (-0, -0),(-0, -1),(-1, -1),(-1, -2) },
                new (int x, int y)[] { (-1, -0),(-0, -0),(-0, -1),(+1, -1) },
            },
            new (int x, int y)[][] { // Sr
                new (int x, int y)[] { (-0, -0),(-0, -1),(+1, -1),(+1, -2) },
                new (int x, int y)[] { (-1, -1),(-0, -1),(-0, -0),(+1, -0) },
            },
            new (int x, int y)[][] {// T
                new (int x, int y)[] { (-0, -0),(-0, -1),(+1, -1),(-0, -2) },
                new (int x, int y)[] { (-1, -0),(-0, -0),(-0, -1),(+1, -0) },
                new (int x, int y)[] { (-1, -1),(-0, -0),(-0, -1),(-0, -2) },
                new (int x, int y)[] { (-1, -1),(-0, -1),(+1, -1),(-0, -0) },
            },
        };

        StageResources resources;
        (Vec pos, int r, int type) current;
        DateTime next_down;
        Random random = new Random();

        public (Vec pos, int r, int type) Current { get => current; }

        public Stage(StageResources resources)
        {
            this.resources = resources;
            State = StageState.Ready;
        }

        void Next()
        {
            current = (((int)Math.Ceiling(resources.Size.x / 2.0), 0), 0, random.Next(1, 7));
            if (CanContinue())
            {
                NextDown();
            }
            else
            {
                Finished();
            }
        }

        private void NextDown()
        {
            next_down = DateTime.Now.AddMilliseconds(500);
        }

        public void Fire()
        {
            switch (State)
            {
                case StageState.Ready:
                    Next();
                    Running();
                    break;
                case StageState.Running:
                    break;
                default:
                    break;
            }
        }

        public void MoveLeft() => MoveOn((-1,0));

        public void MoveRight() => MoveOn((+1, 0));

        public void MoveUp() => MoveOn((0,0), +1);

        public void MoveDown() => MoveOn((0, 1));

        private void MoveOn(Vec v, int dr = 0)
        {
            if (State != StageState.Running) return;

            (Vec pos, int r, int type) next = (current.pos + v, (current.r + dr) % draw[current.type].GetLength(0), current.type);

            var nextDraw = draw[next.type][next.r];
            resources.ClearGhost1();
            for (var i = 0; i < nextDraw.Length; i++)
            {
                var n = nextDraw[i];
                var ny = next.pos.y + n.y;
                if (ny < 0) continue; // if exit above...no problem: rotating on top...
                var nx = next.pos.x + n.x;
                if (nx < 0)
                {
                    // cannot move
                    return;
                }
                if (nx >= resources.Size.x)
                {
                    // cannot move
                    return;
                }
                if (ny >= resources.Size.y)
                {
                    // cannot move - next please
                    Commit();
                    Next();
                    return;
                }
                var idx = ny * resources.Size.x + nx;
                if (idx >= resources.DefaultLayer.Length)
                {
                    // cannot move - next please
                    Commit();
                    Next();
                    return;
                }
                if (resources.DefaultLayer[idx] != 0)
                {
                    // cannot move - next please
                    // current commit
                    Commit();
                    Next();
                    return;
                }
                resources.GhostLayer1[idx] = next.type;
            }
            current = next;
            resources.SwapGhosts();
            NextDown();
        }

        private void Complete()
        {
            while (true)
            { 
            }
        }

        private bool CanContinue()
        {
            var nextDraw = draw[current.type][current.r];
            for (var i = 0; i < nextDraw.Length; i++)
            {
                var n = nextDraw[i];
                var ny = current.pos.y + n.y;
                if (ny < 0) continue; // if exit above...no problem: rotating on top...
                var nx = current.pos.x + n.x;
                if (nx < 0)
                {
                    // cannot move
                    return false;
                }
                if (nx >= resources.Size.x)
                {
                    // cannot move
                    return false;
                }
                if (ny >= resources.Size.y)
                {
                    // cannot move - current please
                    return false;
                }
                var idx = ny * resources.Size.x + current.pos.x + nx;
                if (idx >= resources.DefaultLayer.Length)
                {
                    // cannot move - current please
                    return false;
                }
                if (resources.DefaultLayer[idx] != 0)
                {
                    // cannot move - current please
                    // current commit
                    return false;
                }
            }

            return true;
        }

        private void Commit()
        {
            var nextDraw = draw[current.type][current.r];
            for (var i = 0; i < nextDraw.Length; i++)
            {
                var n = nextDraw[i];
                var ny = current.pos.y + n.y;
                if (ny < 0) continue; // if exit above...no problem: rotating on top...
                var nx = current.pos.x + n.x;
                var idx = ny * resources.Size.x + nx;
                resources.DefaultLayer[idx] = current.type;
            }

            // clear ghost
            resources.ClearGhost(); // so doesn't take ghost layer info

            // now check if some rows complete

            var offset_y = resources.DefaultLayer.Length - resources.Size.x;
            while (true)
            {
                var count = 0;
                for (var x = 0; x < resources.Size.x; x++)
                {
                    if (resources.DefaultLayer[offset_y + x] > 0) 
                        count++;
                }
                if (count == 0) break; // exit condition
                if (count == resources.Size.x) // line complete!
                {
                    for (var o = offset_y + resources.Size.x - 1; o >= resources.Size.x; o--) // copy all elements one line down (as an array)
                    {
                        resources.DefaultLayer[o] = resources.DefaultLayer[o - resources.Size.x];
                    }
                    for (var o = 0; o < resources.Size.x; o++) // clean the first row (top)
                    {
                        resources.DefaultLayer[o] = 0;
                    }
                    // and keep y the same and check it again (it new - the last top)
                }
                else // line is not complete so up 1
                {
                    offset_y -= resources.Size.x;
                }
            }
        }

        public void Update()
        {
            switch (State)
            {
                case StageState.Running:
                    UpdateRunning();
                    break;
                default:
                    break;
            }
        }

        private void UpdateRunning()
        {
            if (DateTime.Now > next_down)
            {
                MoveDown();
                NextDown();
            }
        }

        public StageState State { get; private set; }

        public Stage Ready()
        {
            State = StageState.Ready;
            return this;
        }

        public Stage Running()
        {
            State = StageState.Running;
            return this;
        }

        public Stage LiveLost()
        {
            State = StageState.LiveLost;
            return this;
        }

        public Stage Finished()
        {
            State = StageState.Finished;
            return this;
        }
    }
}
