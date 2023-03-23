using System;
public interface IEndTurn
{
    public (Player prev, Player current) EndTurn();
}