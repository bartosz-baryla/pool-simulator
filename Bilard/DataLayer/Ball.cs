using System;

namespace DataLayer
{
    /* Interfejs kuli */
    public interface IBall
    {
        /** Funkcja odpowiedzialna za poruszenie kuli, obsłużenie odbicia od ścianki */
        void Move();
        double Angle { get; set; }
        int R { get; set; }
        int Y { get; set; }
        int X { get; set; }

    }

    public class Ball : IBall
    {
        /** Położenie kuli oraz jej promień. **/
        private int x, y, r;
        /** Kąt pod jakim leci kula. **/
        private double angle;

        double IBall.Angle
        {
            get => angle;
            set
            {
                angle = value;
            }
        }

        int IBall.R
        {
            get => r;
            set
            {
                r = value;
            }
        }

        int IBall.Y
        {
            get => y;
            set
            {
                y = value;
            }
        }

        int IBall.X
        {
            get => x;
            set
            {
                x = value;
            }
        }

        public Ball(int x, int y, int r, double angle)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.angle = angle;
        }

        public void Move()
        {
            Random random = new Random();

            double dx = Math.Cos(angle) * 4;
            double dy = Math.Sin(angle) * 4;
            x += (int)dx;
            y += (int)dy;

            bool colsion = false;
            
            //sprawdza zderzenie z prawą krawędzią
            if (x + r >= (800 - (2*r)))
            {
                x = (800 - (2 * r)) - r;
                angle = Math.PI - angle;
                colsion = true;
            }
            // sprawdza zdeżenie z lewą krawędzią
            else if (x - r <= 0)
            {
                x = r;
                angle = Math.PI - angle;
                colsion = true;
            }
            // sprawdza zderzenie z dolną krawędzią
            else if (y + r >= (400 - (2 * r)))
            {
                y = (400 - (2 * r)) - r;
                angle = -angle;
                colsion = true;
            }
            // sprawdza zderzenie z górną krawędzią
            else if (y - r <= 0)
            {
                y = r;
                angle = -angle;
                colsion = true;
            }
            /* Przy zderzeniu zmieniamy kąt podróży kuli */
            if (colsion)
            {
                angle += random.NextDouble() * 90;
            }

        }

    }
}
