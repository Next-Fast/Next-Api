using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Buttons;

public class NextButton : INextButton
{
    public ButtonBase Base { get; set; }
}