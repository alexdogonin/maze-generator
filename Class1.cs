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
        public Borders Borders {get; set;}
    }

    public class Generator {
        public static Cell[,] Generate(int width, int length, (int w, int l)? enter = null, (int w, int l)? exit = null) {
            var p = (width + length) * 2 - 1;
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

            if (enter is null) {
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

            var path = new LinkedList<List<(int w, int l)>>();

            path.AddFirst(new List<(int w, int l)>{enter.Value});

            var usedPoints = new HashSet<(int w, int l)>();

            (int w, int l) curPoint = enter.Value; 
            usedPoints.Add(curPoint);
            while(true) {
                var availablePoints = new List<(int, int)>();

                //left
                (int w, int l) next = (curPoint.w - 1, curPoint.l);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (!usedPoints.Contains(next)){
                        availablePoints.Add(next);
                    }
                }

                //right
                next = (curPoint.w + 1, curPoint.l);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (!usedPoints.Contains(next)){
                        availablePoints.Add(next);
                    }
                }

                //forw
                next = (curPoint.w, curPoint.l+1);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (!usedPoints.Contains(next)){
                        availablePoints.Add(next);
                    }
                }

                //back
                next = (curPoint.w - 1, curPoint.l-1);
                if (next.l >= 0 && next.l < length && next.w >= 0 && next.w < width) {
                    if (!usedPoints.Contains(next)){
                        availablePoints.Add(next);
                    }
                }

                if (availablePoints.Count == 0){

                }
                
            }
            
            return null;
        }
    }
}
