
using System.Collections.Generic;
using System.Drawing;
using System.Threading; 

public interface House
{
    int height { get; }
    int floors { get; }
    double fl_height { get; }
}
public interface Elevator
{   bool doorOpen { get; set; }
    bool ElevatorMove { get; set; }
    int loadcapacity { get; set; }
    int floor { get; set; }
    double speed { get; set; }
    double time { get; set; }
    int sel_fl { get; set; }

    void fl_selection(int floors, double fl_height);
    void MoveUp(int floors, double fl_height);
    void MoveDown(int floors, double fl_height);
    void Sensor(bool entry);
    void OpenDoors();
    void CloseDoors();
}

public interface Floor
{
    int Num_Fl { get; set; }
    int Cab1_Fl { get; set; }
    int Cab2_Fl { get; set; }
    string Cab1_St { get; set; }
    string Cab2_St { get; set; }
    bool El_Call { get; set; }
    void Call(Elevators elevator, ThisHouse house);
}

public interface Passenger
{
    int CurFl { get; set; }
    int CallFl { get; set; }
    bool entry { get; set; }
    string choice { get; set; }
    void CallElevator(Floors floor, Elevators elevator1, Elevators elevator2, ThisHouse house);
    void Enter(Elevators elevator);
    void Has_Entered(Elevators elevator);
    void ElevatorInteraction(Elevators elevator, ThisHouse house);
    void Out(Elevators elevator);
}
public class ThisHouse : House
{
    public int height => 60;
    public int floors => 20;
    public double fl_height => (double)height / (double)floors;
}


public class Elevators : Elevator
{
    public bool doorOpen { get; set; }
    public bool ElevatorMove { get; set; }
    public int loadcapacity { get; set; }
    public int floor { get; set; }
    public double speed { get; set; }
    public double time { get; set; }
    public int sel_fl { get; set; }

    public void MoveUp(int floors, double fl_height)
    {
        if (floor != floors)
        {

            if (ElevatorMove == false)
            {
                OpenDoors();
                CloseDoors();
            }
            ElevatorMove = true;
            time = fl_height/speed;

            Thread.Sleep((int)(time * 1000));
            floor++;
            if (floor == sel_fl)
            {
                ElevatorMove = false;
                OpenDoors();
            }
        }
    }
    public void MoveDown(int floors, double fl_height)
    {
        if (floor != floors)
        {
            if (ElevatorMove == false)
            {
                OpenDoors();
                CloseDoors();
            }
            ElevatorMove = true;
            time = fl_height / speed;

            Thread.Sleep((int)(time * 1000));
            floor--;
            if (floor == sel_fl)
            {
                ElevatorMove = false;
                OpenDoors();
            }
        }
    }
    public void fl_selection(int floors, double fl_height)
    {
        if (sel_fl > floor && floor != floors)
        {
            MoveUp(floors, fl_height);
        }
        if (sel_fl < floor && floor != 1)
        {
            MoveDown(floors, fl_height);
        }
    }
    public void OpenDoors()
    {
        if (doorOpen != true)
        {
            Console.WriteLine("Двери открываются");
            doorOpen = true;
        }
        else
        {
            Console.WriteLine("Двери открыты");
        }
    }
    public void CloseDoors()
    {
        if (doorOpen != true)
        {
            Console.WriteLine("Двери закрыты");
        }
        else
        {
            Console.WriteLine("Двери закрываются");
            doorOpen = false;
        }
    }
    public void Sensor(bool entry)
    {
        if (entry)
        {
            Console.WriteLine("Датчики кабины зафиксировали движение между дверьми");
        }
        if (!entry)
        {
            Console.WriteLine("Датчики кабины зафиксировали отсутсвия движения между дверьми");
        }
    }
}


public class Floors : Floor
{
    public int Num_Fl { get; set; }
    public int Cab1_Fl { get; set; }
    public int Cab2_Fl { get; set; }
    public string Cab1_St { get; set; }
    public string Cab2_St { get; set; }
    public bool El_Call { get; set; }
    public void Call(Elevators elevator, ThisHouse house)
    {
        El_Call = true;
        elevator.fl_selection(house.floors, house.fl_height);
    }

}


public class Passengers : Passenger
{
    public int CurFl { get; set; }
    public int CallFl { get; set; }
    public bool entry { get; set; }
    public string choice { get; set; }
    public void CallElevator(Floors floor, Elevators elevator1, Elevators elevator2, ThisHouse house)
    {
        floor.Cab1_Fl = elevator1.floor;
        floor.Cab1_St = "Первый Лифт стоит на " + floor.Cab1_Fl + " Этаже.\nЕго скорость: " + elevator1.speed + "\nГрузоподъемность: " + elevator1.loadcapacity;
        floor.Cab2_Fl = elevator2.floor;
        floor.Cab2_St = "Первый Лифт стоит на " + floor.Cab2_Fl + " Этаже.\nЕго скорость: " + elevator2.speed + "\nГрузоподъемность: " + elevator2.loadcapacity;

        Console.WriteLine("Пассажир находится на " + CurFl + " Этаже, какой лифт ему вызвать?");
        Console.WriteLine("Состояния Лифта №1:\n" + floor.Cab1_St + "\n");
        Console.WriteLine("Состояния Лифта №2:\n" + floor.Cab2_St + "\n");
        Console.WriteLine("Введите 1 или 2 для выбора лифта");
        choice = Console.ReadLine();
        if (choice == "1")
        {
            floor.Cab1_St = "Первый Лифт стоит на ";
            Console.WriteLine("Пассажир вызвал лифт на " + CurFl + " Этаж");

            elevator1.sel_fl = CurFl;
            Console.WriteLine(floor.Cab1_St + floor.Cab1_Fl + " этаже");

            while (elevator1.floor != elevator1.sel_fl)
            {
                floor.Call(elevator1, house);
                floor.Cab1_Fl = elevator1.floor;
                if (elevator1.ElevatorMove)
                {
                    floor.Cab1_St = "Первый Лифт движется, сейчас он на ";
                }
                else
                {
                    floor.Cab1_St = "Первый Лифт стоит на ";
                }
                Console.WriteLine(floor.Cab1_St + floor.Cab1_Fl + " этаже");
            }
        }
        else
        {
            Console.WriteLine("Пассажир вызвал лифт на " + CurFl+" Этаж");

            floor.Cab2_St = "Второй Лифт стоит на ";
            elevator2.sel_fl = CurFl;
            Console.WriteLine(floor.Cab2_St + floor.Cab2_Fl + " этаже");

            while (elevator2.floor != elevator2.sel_fl)
            {
                floor.Call(elevator2, house);
                floor.Cab2_Fl = elevator2.floor;
                if (elevator2.ElevatorMove)
                {
                    floor.Cab2_St = "Второй Лифт движется, сейчас он на ";
                }
                else
                {
                    floor.Cab2_St = "Второй Лифт стоит на ";
                }
                Console.WriteLine(floor.Cab2_St + floor.Cab2_Fl + " этаже");
            }
        }
    }
    public void Enter(Elevators elevator)
    {
        entry = true;
        elevator.Sensor(entry);
        Console.WriteLine("Пассажир входит в лифт");
        Thread.Sleep(2000);
        Has_Entered(elevator);
        Console.WriteLine("Пассажир вошел в лифт");
    }
    public void Has_Entered(Elevators elevator)
    {
        entry = false;
        elevator.Sensor(entry);
    }
    public void ElevatorInteraction(Elevators elevator, ThisHouse house)
    {
        Console.WriteLine("Пассажир отправил лифт на " + CallFl +" Этаж");
        elevator.sel_fl = CallFl;
        if (choice == "1")
        {
            Console.WriteLine("Первый лифт с пассажиром находится на " + CurFl);
            int floor;
            string text;
            while (elevator.floor != elevator.sel_fl)
            {

                elevator.fl_selection(house.floors, house.fl_height);
                floor = elevator.floor;
                if (elevator.ElevatorMove)
                {
                    text = "Первый Лифт движется, сейчас он на ";
                }
                else
                {
                    text = "Первый Лифт стоит на ";
                }
                Console.WriteLine(text + floor + " этаже");
            }
        }
        else
        {
            Console.WriteLine("Второй лифт с пассажиром находится на " + CurFl);
            int floor;
            string text;
            while (elevator.floor != elevator.sel_fl)
            {

                elevator.fl_selection(house.floors, house.fl_height);
                floor = elevator.floor;
                if (elevator.ElevatorMove)
                {
                    text = "Второй Лифт движется, сейчас он на ";
                }
                else
                {
                    text = "Второй Лифт стоит на ";
                }
                Console.WriteLine(text + floor + " этаже");
            }
        }
    }
    public void Out(Elevators elevator)
    {
        entry = true;
        elevator.Sensor(entry);
        Console.WriteLine("Пассажир выходит из лифта");
        Thread.Sleep(2000);
        Has_Entered(elevator);
        Console.WriteLine("Пассажир вышел из лифта");
    }
}


class program
{

    static public void Main()
    {
        ThisHouse house = new ThisHouse();

        Elevators Elevator1 = new Elevators();
        Elevator1.doorOpen = true;
        Elevator1.loadcapacity = 400;
        Elevator1.speed = 1.52;
        Elevator1.floor = 1;
        Elevator1.ElevatorMove = false;

        Elevators Elevator2 = new Elevators();
        Elevator2.doorOpen = true;
        Elevator2.loadcapacity = 800;
        Elevator2.speed = 0.76;
        Elevator2.floor = 1;
        Elevator2.ElevatorMove = false;


        Floors[] floor = new Floors[20];
        for (int i = 0; i < house.floors; i++)
        {
            floor[i] = new Floors();
            floor[i].Num_Fl = i + 1;
            floor[i].El_Call = false;
            floor[i].Cab1_Fl = Elevator1.floor;
            floor[i].Cab2_Fl = Elevator2.floor;
        }


        Passengers[] Pass = new Passengers[3];

        Pass[0] = new Passengers { CurFl = 1, CallFl = 14 };
        Pass[0].CallElevator(floor[Pass[0].CurFl - 1], Elevator1, Elevator2, house);
        if (Pass[0].choice == "1")
        {
            Pass[0].Enter(Elevator1);
            Pass[0].ElevatorInteraction(Elevator1, house);
            Pass[0].Out(Elevator1);
        }
        else
        {
            Pass[0].Enter(Elevator2);
            Pass[0].ElevatorInteraction(Elevator2, house);
            Pass[0].Out(Elevator2);
        }

        Pass[1] = new Passengers { CurFl = 15, CallFl = 7 };
        Pass[1].CallElevator(floor[Pass[0].CurFl - 1], Elevator1, Elevator2, house);
        if (Pass[1].choice == "1")
        {
            Pass[1].Enter(Elevator1);
            Pass[1].ElevatorInteraction(Elevator1, house);
            Pass[1].Out(Elevator1);
        }
        else
        {
            Pass[1].Enter(Elevator2);
            Pass[1].ElevatorInteraction(Elevator2, house);
            Pass[1].Out(Elevator2);
        }

        Pass[2] = new Passengers { CurFl = 4, CallFl = 6 };
        Pass[2].CallElevator(floor[Pass[2].CurFl - 1], Elevator1, Elevator2, house);
        if (Pass[2].choice == "1")
        {
            Pass[2].Enter(Elevator1);
            Pass[2].ElevatorInteraction(Elevator1, house);
            Pass[2].Out(Elevator1);
        }
        else
        {
            Pass[2].Enter(Elevator2);
            Pass[2].ElevatorInteraction(Elevator2, house);
            Pass[2].Out(Elevator2);
        }

    }
}
