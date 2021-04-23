using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RentalVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Movie movie1 = new Movie("ターミネータ2", Movie.REGULAR);
            Movie movie2 = new Movie("タイタニック", Movie.NEW_RELEASE);
            Movie movie3 = new Movie("トイストーリー", Movie.CHILDRENS);

            Rental rental1 = new Rental(movie1, 5);
            Rental rental2 = new Rental(movie2, 9);
            Rental rental3 = new Rental(movie3, 4);

            Customer customer = new Customer("すぎさく");
            customer.addRental(rental1);
            customer.addRental(rental2);
            customer.addRental(rental3);

            Console.WriteLine(customer.statement());
        }
    }

    public class Movie {
        public const int CHILDRENS = 2;
        public const int REGULAR = 0;
        public const int NEW_RELEASE = 1;

        private string _title;
        private int _priceCode;

        public Movie(string title, int priceCode){
            _title = title;
            _priceCode = priceCode;
        }

        public int getPriceCode(){
            return _priceCode;
        }
        public void setpriceCode(int arg){
            _priceCode = arg;
        }
        public string getTitle(){
            return _title;
        }
    }

    public class Rental {
        private Movie _movie;
        private int _daysRented;

        public Rental(Movie movie, int daysRented){
            _movie = movie;
            _daysRented = daysRented;
        }
        public int getDaysRented(){
            return _daysRented;
        }
        public Movie getMovie(){
            return _movie;
        }
        internal double getCharge()
        {
            double result = 0;
            switch (getMovie().getPriceCode())
            {
                case Movie.REGULAR:
                    result += 2;
                    if (getDaysRented() > 2)
                        result += (getDaysRented() - 2) * 1.5;
                    break;
                case Movie.NEW_RELEASE:
                    result += getDaysRented() * 3;
                    break;
                case Movie.CHILDRENS:
                    result += 1.5;
                    if (getDaysRented() > 3)
                        result += (getDaysRented() - 3) * 1.5;
                    break;
            }
            return result;
        }
    }

    public class Customer{
        private string _name;
        private ArrayList _rentals = new ArrayList();

        public Customer (string name){
            _name = name;
        }

        public void addRental(Rental arg){
            _rentals.Add(arg);
        }
        public string getName(){
            return _name;
        }

        public string statement(){
            double totalAmount = 0;
            int frequentRenterPoints = 0;
            IEnumerator rentals = _rentals.GetEnumerator();
            string result = "Rental Record for " + getName() + "\n";
            while(rentals.MoveNext()){
                Rental each = (Rental)rentals.Current;

                //レンタルポイントを加算
                frequentRenterPoints++;
                //新作を二日以上借りた場合はボーナスポイント
                if((each.getMovie().getPriceCode() == Movie.NEW_RELEASE) && 
                    each.getDaysRented() > 1) frequentRenterPoints++;

                //この貸し出しに関する数値の表示
                result += "\t" + each.getMovie().getTitle() + "\t" + each.getCharge().ToString() + "\n";
                totalAmount += each.getCharge();
            }
            //フッダ部分の追加
            result += "Amount owed is " + totalAmount.ToString() + "\n";
            result += "You earned " + frequentRenterPoints.ToString() + " frequent renter points";
            return result;
        }
        private double amountFor(Rental aRental){
            return aRental.getCharge();
        }
    }
}
