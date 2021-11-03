using System;
using System.Collections.Generic;

namespace maze_generator
{
    public enum Borders {
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }

    public enum Direction {
        Left = 1,
        Right = 2,
        Forward = 4,
        Back = 8
    }
    
    public class Cell
    {
        public (int x, int y)? Prev {get; set;}
        public List<(int x, int y)> Next {get; set;}
    }

    public class Generator {
        public static Cell[,] Generate(int width, int length, (int w, int l)? enter = null, (int w, int l)? exit = null) {
            var rand = new Random();

            if (enter is null) {
                var sideInd = rand.Next(3);

                enter = sideInd switch {
                    0 => (rand.Next(length - 1), 0),
                    1 => (length - 1, rand.Next(width - 1)),
                    2 => (rand.Next(length - 1), width -1),
                    3 => (0, rand.Next(width - 1)),
                };
            }

            if (exit is null) {
                var sideInd = rand.Next(3);
                int w = 0, l = 0;

                switch (sideInd) {
                    case 0: { 
                        l = rand.Next(length - 1);
                        w =  0;
                        break;
                    }
                    case 1: {
                        l = length - 1;
                        w = rand.Next(width - 1);
                        break;
                    }
                    case 2: {
                        l = rand.Next(length - 1); 
                        w = width -1;
                        break;
                    }
                    case 3: {
                        l = 0; 
                        w = rand.Next(width - 1);
                        break;
                    }
                };
                exit = (w, l);

                if (exit == enter) {
                    if (w == 0 || w == (width -1))
                        if (l == 0) {
                            l++;
                        } else 
                            l--;
                    else {
                        if (w == 0) {
                            w++;
                        } else 
                            w--;
                    }

                    exit = (w, l);
                }
            }

            var maze = new Cell[width, length];

            (int w, int l) curPoint = enter.Value; 
            
            maze[curPoint.w, curPoint.l] = new Cell();
            int usedPoints = 1;

            while(usedPoints < width*length) {

                var availablePoints = new List<(int, int)>();

                //left
                (int w, int l) next = (curPoint.w - 1, curPoint.l);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (maze[next.w, next.l] is null){
                        availablePoints.Add(next);
                    }
                }

                //right
                next = (curPoint.w + 1, curPoint.l);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (maze[next.w, next.l] is null){
                        availablePoints.Add(next);
                    }
                }

                //forw
                next = (curPoint.w, curPoint.l+1);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (maze[next.w, next.l] is null){
                        availablePoints.Add(next);
                    }
                }

                //back
                next = (curPoint.w - 1, curPoint.l-1);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (maze[next.w, next.l] is null){
                        availablePoints.Add(next);
                    }
                }

                if (availablePoints.Count == 0){
                    var c = maze[curPoint.w, curPoint.l];
                    if (c.Prev is null) 
                        break;

                    curPoint = c.Prev.Value;
                    continue;
                }

                var nextInd = rand.Next(availablePoints.Count);
                (int w, int l) nextPoint = availablePoints[nextInd];
                maze[nextPoint.w, nextPoint.l] = new Cell{
                    Prev = curPoint
                };
                usedPoints++;   

                maze[curPoint.w, curPoint.l].Next.Add(nextPoint);

                if (exit.Value == nextPoint) continue;

                curPoint = nextPoint;
            }
            
            return maze;
        }
    }
}
