using NUnit.Framework;
using System;
using vproker.Services;

namespace vproker_tests
{
    [TestFixture]
    public class PaymentCalculationTests
    {
        [Test]
        public void CalculatesWorkShip()
        {
            (var type, var total, var days, var delayedHours) = PaymentCalculation.Calculate(new DateTime(2019, 1, 1), new DateTime(2019, 1, 1), 800, 500);
            Assert.AreEqual(type, PaymentType.WorkShift);
            Assert.AreEqual(total, 500);
        }

        [Test]
        public void CalcOnAnotherDay()
        {
            (var type, var total, var days, var delayedHours) = PaymentCalculation.Calculate(new DateTime(2019, 1, 1, 18, 30, 0), new DateTime(2019, 1, 2, 9, 0, 0), 800, 500);
            Assert.AreEqual(type, PaymentType.Days);
            Assert.AreEqual(total, 800);
        }

        [Test]
        public void Calc1Day()
        {
            var payment = PaymentCalculation.Calculate(new DateTime(2019, 1, 1), new DateTime(2019, 1, 2), 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 800);
        }

        [Test]
        public void Calc3Days()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 4, 10, 0, 0);
            var payment = PaymentCalculation.Calculate(start, end, 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 2240);
        }

        [Test]
        public void Calc6Days()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 7, 10, 0, 0);
            var payment = PaymentCalculation.Calculate(start, end, 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 4080);
        }

        [Test]
        public void Calc9Days()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 10, 10, 0, 0);
            var payment = PaymentCalculation.Calculate(start, end, 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 5680);
        }

        [Test]
        public void Calc13Days()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 13, 10, 0, 0);
            var payment = PaymentCalculation.Calculate(start, end, 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 7040);
        }


        [Test]
        public void Calc15Days()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 16, 10, 0, 0);
            var payment = PaymentCalculation.Calculate(start, end, 800, 500);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 8160);
        }

        [Test]
        public void Calc1HourDelay()
        {
            DateTime start = new DateTime(2019, 1, 1, 12, 30, 0);
            DateTime end = new DateTime(2019, 1, 2, 12, 40, 0);
            Price pay = new Price(800, 500, 100);

            var payment = PaymentCalculation.Calculate(start, end, pay);
            Assert.AreEqual(payment.Type, PaymentType.DaysAndHours);
            Assert.AreEqual(payment.DelayedHours, 1);
            Assert.AreEqual(payment.Total, 900);
        }

        [Test]
        public void Calc2HoursDelay()
        {
            DateTime start = new DateTime(2019, 1, 1, 11, 30, 0);
            DateTime end = new DateTime(2019, 1, 2, 12, 40, 0);
            Price pay = new Price(800, 500, 100);

            var payment = PaymentCalculation.Calculate(start, end, pay);
            Assert.AreEqual(payment.Type, PaymentType.DaysAndHours);
            Assert.AreEqual(payment.DelayedHours, 2);
            Assert.AreEqual(payment.Total, 1000);
        }

        [Test]
        public void Calc3HoursDelay()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 2, 13, 0, 0);
            Price pay = new Price(800, 500, 100);

            var payment = PaymentCalculation.Calculate(start, end, pay);
            Assert.AreEqual(payment.Type, PaymentType.DaysAndHours);
            Assert.AreEqual(payment.DelayedHours, 3);
            Assert.AreEqual(payment.Total, 1100);
        }

        // no hours delay more than 4 hours
        [Test]
        public void Max4HoursDelay()
        {
            DateTime start = new DateTime(2019, 1, 1, 10, 0, 0);
            DateTime end = new DateTime(2019, 1, 2, 15, 0, 0);
            Price pay = new Price(800, 500, 100);

            var payment = PaymentCalculation.Calculate(start, end, pay);
            Assert.AreEqual(payment.Type, PaymentType.Days);
            Assert.AreEqual(payment.Total, 1600);
        }

        [Test]
        public void NoHoursDelayInTheSameDay()
        {
            DateTime start = new DateTime(2019, 1, 6, 22, 20, 0);
            DateTime end = new DateTime(2019, 1, 7, 2, 0, 0);
            Price pay = new Price(770, 670, 150);

            var payment = PaymentCalculation.Calculate(start, end, pay);
            Assert.AreEqual(payment.Days, 1);
            Assert.AreEqual(payment.Total, 770);
            Assert.AreEqual(payment.Type, PaymentType.Days);
        }
    }
}
