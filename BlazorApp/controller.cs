using MyModel;
using MyView;

namespace MyController;
public class Controller: IController {
    readonly ICalculator Simple;

    public Controller(ICalculator simple) {
        this.Simple = simple;
    }

    public History[] GetHistory() {
        HistoryEntity[] entity = Simple.GetHistory();
        History[] result = entity.Select(x => Entity2History(x)).ToArray();
        return result;
    }

    public double GetAnwser(double x, double y, string op) {
        string formula = String.Format("{0:#.########} {1} {2:#.########}", x, op, y);
        double anwser = Simple.Execute(formula);
        return anwser;
    }

    private static History Entity2History(HistoryEntity entity) {
        string formula = String.Format("{0} = ", entity.formula);
        double result = entity.result;
        return new History(formula, result);
    }
}
