using Microsoft.Extensions.Hosting;
using MyView;
using MyModel;

namespace Test
{
    [TestClass]
    public class Controller
    {
        [TestMethod]
        public void CorrectTypeCastFromModel()
        {
            List<HistoryEntity> sethistory = new(0);
            sethistory.Add(new HistoryEntity("1 + 1", 2));
            sethistory.Add(new HistoryEntity("2 - 4", -2));

            List<History> expected = new(0);
            expected.Add(new History("1 + 1 = ", 2));
            expected.Add(new History("2 - 4 = ", -2));

            SpyCalculator calculator = new();
            calculator.SetHistory(sethistory.ToArray());
            MyController.Controller controller = new(calculator);
            History[] actual = controller.GetHistory();

            Assert.AreEqual(expected.ToArray()[0].Formula, actual[0].Formula);
            Assert.AreEqual(expected.ToArray()[1].Formula, actual[1].Formula);
            Assert.AreEqual(expected.ToArray()[0].Result, actual[0].Result);
            Assert.AreEqual(expected.ToArray()[1].Result, actual[1].Result);
        }

        [TestMethod]
        public void CallModelSimpleCaculator()
        {
            SpyCalculator calculator = new();
            MyController.Controller controller = new(calculator);
            controller.GetAnwser(1, 2, "+");
            controller.GetHistory();

            Assert.AreEqual(2, calculator.GetCall());
        }

        [TestMethod]
        public void CallFormatIsCorrect()
        {
            SpyCalculator calculator = new();
            MyController.Controller controller = new(calculator);
            controller.GetAnwser(1, 2, "/");

            Assert.AreEqual("1 / 2", calculator.GetFormula());
        }
    }

    [TestClass]
    public class Operator
    {
        [TestMethod]
        public void CorrectAddition()
        {
            AddOperator op = new();
            double expect = 5;
            double actual = op.Execute(2, 3);

            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void CorrectSubtraction()
        {
            SubtractOperator op = new();
            double expect = 5;
            double actual = op.Execute(8, 3);

            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void CorrectMultiplication()
        {
            MultiplyOperator op = new();
            double expect = 5;
            double actual = op.Execute(2.5, 2);

            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void CorrectDivision()
        {
            DivideOperator op = new();
            double expect = 5;
            double actual = op.Execute(10, 2);

            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void CorrectDivisionByZero()
        {
            DivideOperator op = new();

            Assert.ThrowsException<InvalidDataException>(() => op.Execute(10, 0));
        }
    }

    [TestClass]
    public class SimpleCalculatorModel
    {
        [TestMethod]
        public void CorrectInputValidation()
        {
            SpyData history = new();
            SimpleCalculator calculator = new(history);

            Assert.ThrowsException<ArgumentException>(() => calculator.Execute("a + 12"));
        }

        [TestMethod]
        public void CorrectArgumentNumberValidation()
        {
            SpyData history = new();
            SimpleCalculator calculator = new(history);

            Assert.ThrowsException<InvalidDataException>(() => calculator.Execute("1 + 2 + 3"));
        }

        [TestMethod]
        public void CorrectDigitsValidation()
        {
            SpyData history = new();
            SimpleCalculator calculator = new(history);

            Assert.ThrowsException<InvalidDataException>(() => calculator.Execute("111111111 + 1"));
        }

        [TestMethod]
        public void CallSaveData()
        {
            SpyData history = new();
            SimpleCalculator calculator = new(history);
            calculator.Execute("1 + 1");

            Assert.AreEqual(1, history.GetCall());
        }
    }

    [TestClass]
    public class DataAccess
    {
        [TestMethod]
        public void CanSaveData()
        {
            MemoryHistory history = new();
            HistoryEntity[] start = history.GetHistory();

            history.Add(new HistoryEntity("1 + 1", 2));
            HistoryEntity expected = new HistoryEntity("1 + 1", 2);

            Assert.AreEqual(0, start.Length);
            Assert.AreEqual(1, history.GetHistory().Length);
            Assert.AreEqual(expected.formula, history.GetHistory()[0].formula);
            Assert.AreEqual(expected.result, history.GetHistory()[0].result);
        }
    }

    public class SpyCalculator: ICalculator
    {
        int call { get; set; } = 0;
        string formula { get; set; } = "";
        HistoryEntity[] sethistory { get; set; } = new HistoryEntity[0];
        public HistoryEntity[] GetHistory()
        {
            call += 1;
            return sethistory;
        }
        public double Execute(string formula)
        {
            call += 1;
            this.formula = formula;
            return 0;
        }
        public void SetHistory(HistoryEntity[] history)
        {
            sethistory = history;
        }

        public int GetCall() {
            return call;
        }

        public string GetFormula() {
            return formula;
        }
    }

    public class SpyData: ICalculationHistory
    {
        int call { get; set; } = 0;
        public void Add(HistoryEntity history)
        {
            call += 1;
        }
        public HistoryEntity[] GetHistory()
        {
            return new HistoryEntity[0];
        }

        public int GetCall() {
            return call;
        }
    }
}
