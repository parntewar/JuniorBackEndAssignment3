namespace MyView;

public class History {
    public string Formula;
    public double Result;
    public History(string formula, double result) {
        this.Formula = formula;
        this.Result = result;
    }
}

public interface IController {
    public History[] GetHistory();
    public double GetAnwser(double x, double y, string op);
}
