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

            //Console.WriteLine(customer.statement());
            Console.WriteLine(customer.htmlStatement());
        }
    }

    public class Movie {
        public const int CHILDRENS = 2;
        public const int REGULAR = 0;
        public const int NEW_RELEASE = 1;

        private string _title;
        private Price _price;

        public Movie(string title, int priceCode){
            _title = title;
            setPriceCode(priceCode);
        }

        public int getPriceCode(){
            return _price.getPriceCode();
        }
        public void setPriceCode(int arg){
            switch (arg)
            {
                case REGULAR:
                    _price = new RegularPrice();
                    break;
                case CHILDRENS:
                    _price = new ChildrensPrice();
                    break;
                case NEW_RELEASE:
                    _price = new NewReleasePrice();
                    break;
                default:
                    throw new InvalidOperationException("不正な料金コード");
            }
        }
        public string getTitle(){
            return _title;
        }

        internal double getCharge(int daysRented)
        {
            return _price.getCharge(daysRented);
        }

        internal int getFrequentRenterPoints(int daysRented)
        {
            return _price.getFrequentRenterPoints(daysRented);
        }
    }

    abstract class Price {
        abstract internal int getPriceCode();
        abstract internal double getCharge(int daysRented);
        internal virtual int getFrequentRenterPoints(int daysRented)
        {
            return 1;
        }
    }

    class ChildrensPrice : Price {
        internal override int getPriceCode(){
            return Movie.CHILDRENS;
        }
        internal override double getCharge(int daysRented)
        {
            double result = 1.5;
            if (daysRented > 3)
                result += (daysRented - 3) * 1.5;
            return result;
        }
    }

    class NewReleasePrice : Price
    {
        internal override int getPriceCode()
        {
            return Movie.NEW_RELEASE;
        }
        internal override double getCharge(int daysRented)
        {
            return daysRented * 3;
        }
        internal override int getFrequentRenterPoints(int daysRented){
            return (daysRented > 1) ? 2 : 1;
        }
    }

    class RegularPrice : Price
    {
        internal override int getPriceCode()
        {
            return Movie.REGULAR;
        }
        internal override double getCharge(int daysRented){
            double result = 2;
            if (daysRented > 2)
                result += (daysRented - 2) * 1.5;
            return result;
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
            return _movie.getCharge(_daysRented);
        }

        internal int getFrequentRenterPoints(){
            return _movie.getFrequentRenterPoints(_daysRented);
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
            IEnumerator rentals = _rentals.GetEnumerator();
            string result = "Rental Record for " + getName() + "\n";
            while(rentals.MoveNext()){
                Rental each = (Rental)rentals.Current;

                //この貸し出しに関する数値の表示
                result += "\t" + each.getMovie().getTitle() + "\t" + each.getCharge().ToString() + "\n";
            }
            //フッダ部分の追加
            result += "Amount owed is " + getTotalCharge().ToString() + "\n";
            result += "You earned " + getTotalFrequentRenterPoints().ToString() + " frequent renter points";
            return result;
        }

        public string htmlStatement()
        {
            IEnumerator rentals = _rentals.GetEnumerator();
            string result = "<H1>Rental Record for <EM>" + getName() + "</EM></H1><P>\n";
            while (rentals.MoveNext())
            {
                Rental each = (Rental)rentals.Current;

                //この貸し出しに関する数値の表示
                result += each.getMovie().getTitle() + ": " + each.getCharge().ToString() + "<BR>\n";
            }
            //フッダ部分の追加
            result += "<P>You owe <EM>" + getTotalCharge().ToString() + "</EM><P>\n";
            result += "On this rental you earned <EM>" + getTotalFrequentRenterPoints().ToString() + "</EM> frequent renter points<P>";
            return result;
        }

        private double amountFor(Rental aRental){
            return aRental.getCharge();
        }

        private double getTotalCharge(){
            double result = 0;
            IEnumerator rentals = _rentals.GetEnumerator();
            while(rentals.MoveNext()){
                Rental each = (Rental)rentals.Current;
                result += each.getCharge();
            }
            return result;
        }

        private int getTotalFrequentRenterPoints(){
            int result = 0;
            IEnumerator rentals = _rentals.GetEnumerator();
            while (rentals.MoveNext())
            {
                Rental each = (Rental)rentals.Current;
                result += each.getFrequentRenterPoints();
            }
            return result;
        }
    }
}
