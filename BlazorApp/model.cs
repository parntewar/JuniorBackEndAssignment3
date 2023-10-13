using System.Text.RegularExpressions;

namespace MyModel;

public interface IOperator {
    public double Execute(double x, double y);
}

public class AddOperator: IOperator {
    public double Execute(double x, double y) {
        return x + y;
    }
}

public class SubtractOperator: IOperator {
    public double Execute(double x, double y) {
        return x - y;
    }
}

public class DivideOperator: IOperator {
    public double Execute(double x, double y) {
        if (y == 0) throw new InvalidDataException("Can't devide by zero.");
        return x / y;
    }
}

public class MultiplyOperator: IOperator {
    public double Execute(double x, double y) {
        return x * y;
    }
}

public class HistoryEntity {
    public string formula;
    public double result;

    public HistoryEntity(string formula, double result) {
        this.formula = formula;
        this.result = result;
    }
}

public interface ICalculationHistory {
    public void Add(HistoryEntity history);
    public HistoryEntity[] GetHistory();
}

public class MemoryHistory: ICalculationHistory {
    readonly List<HistoryEntity> history = new(0);

    public void Add(HistoryEntity history) {
        this.history.Add(history);
    }

    public HistoryEntity[] GetHistory() {
        return history.ToArray();
    }
}

public interface ICalculator
{
    public double Execute(string formula);
    public HistoryEntity[] GetHistory();

}
public class SimpleCalculator: ICalculator {
    private IOperator? Operator {get; set;}
    private ICalculationHistory history;

    public SimpleCalculator(ICalculationHistory history) {
        this.history = history;
    }

    public double Execute(string formula) {
        if (!ContainString(formula)) throw new ArgumentException("Wrong Input.");
        string[] split = formula.Split(" ");
        if (split.Length != 3) throw new InvalidDataException("Accept only 3 parameters.");
        double x = DoubleParse(split[0]);
        string op = split[1];
        double y = DoubleParse(split[2]);
        double result;
        if (op == "+") {
            Operator = new AddOperator();
        }
        if (op == "-") {
            Operator = new SubtractOperator();
        }
        if (op == "*") {
            Operator = new MultiplyOperator();
        }
        if (op == "/") {
            Operator = new DivideOperator();
        }
        result = Operator!.Execute(x, y);
        history.Add(new HistoryEntity(formula, result));
        return result;
    }

    public HistoryEntity[] GetHistory() {
        return history.GetHistory();
    }

    private bool ContainString(string formula) {
        string acceptable = @"^[0-9+\-*/\s\.]+$";
        Match match = Regex.Match(formula, acceptable);
        return match.Success;
    }

    private double DoubleParse(string x) {
        Match dot = Regex.Match(x, "\\.");
        if (!dot.Success) {
            if (x.Length > 8) throw new InvalidDataException("Digits more than 8.");
            return Double.Parse(x);
        }
        string[] numbers = x.Split(".");
        if (numbers[0].Length > 8) throw new InvalidDataException("Digits more than 8.");
        numbers[1] = Math.Round(Double.Parse("0." + numbers[1]), 8).ToString("#0.########");
        numbers[1] = numbers[1].Split(".")[1];
        return Double.Parse(numbers[0] + "." + numbers[1]);
    }
}
