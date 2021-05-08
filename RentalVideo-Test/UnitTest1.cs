using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentalVideo;

namespace RentalVideo_Test
{
    [TestClass]
    public class UnitTest1
    {   
        [TestMethod]
        public void StatementTest()
        {
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

            string result = "Rental Record for すぎさく"+ "\n";
            result += "\t" + "ターミネータ2" + "\t" +  "6.5"+ "\n";
            result += "\t" + "タイタニック" + "\t" + "27"+ "\n";
            result += "\t" + "トイストーリー" + "\t" + "3"+ "\n";
            result += "Amount owed is 36.5"+ "\n";
            result += "You earned 4 frequent renter points";

            Assert.AreEqual(customer.statement(), result);
        }

        [TestMethod]
        public void HtmlstatementTest()
        {
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

            string result = "<H1>Rental Record for <EM>すぎさく</EM></H1><P>" + "\n";
            result += "ターミネータ2: 6.5<BR>" + "\n";
            result += "タイタニック: 27<BR>" + "\n";
            result += "トイストーリー: 3<BR>" + "\n";
            result += "<P>You owe <EM>36.5</EM><P>" + "\n";
            result += "On this rental you earned <EM>4</EM> frequent renter points<P>";

            Assert.AreEqual(customer.htmlStatement(), result);
        }
    }
}
