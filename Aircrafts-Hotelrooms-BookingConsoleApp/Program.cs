// This is an application to book a hotel room and an aircraft seat automatically
// You can add your own aircraft files and room files, make sure you met the requirements
// Aircraft requirements = a folder called Aircraft with subfolders inside (Plane, Helicopter)
// Inside these subfolders will the name and the seats of the aircraft
// Symbols for the seats = hyphen dash ( - ) is unoccupied, uppercased ( X ) is occupied.

namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    internal class Program
    {
        //location of the folders
        static string rootFolderAircrafts = @"C:\Aircrafts-Hotelrooms-BookingConsoleApp\Aircrafts-Hotelrooms-BookingConsoleApp\bin\Debug\net9.0\Aircrafts\";
        static string rootFolderHotelrooms = @"C:\Aircrafts-Hotelrooms-BookingConsoleApp\Aircrafts-Hotelrooms-BookingConsoleApp\bin\Debug\net9.0\Hotelrooms\";
        static void Main(string[] args)
        {
            hotelroomDict = LoadHotelroomFolder();
            aircraftDict = LoadAircraftsFromFolder();

            int choice = -1;
            do
            {
                Console.Clear();
                MainMenu();
                try
                {
                    choice = AskMenuOptions();
                    switch (choice) // MTR - Switch
                    {
                        case 1:
                            HotelRooms();
                            break;
                        case 2:
                            Aircrafts();
                            break;
                        case 0:
                            break;
                        default:
                            Console.WriteLine("Input out of range!");
                            Console.ReadLine();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.ReadLine();
                }
            } while (choice != 0);
        }
        static Dictionary<string, Hotelroom> hotelroomDict; //MTR - Dictionary
        static Dictionary<string, Aircraft> aircraftDict; //MTR – Dictionary
        static void HotelRooms()//methode for hotelroom logistics
        {
            List<Hotelroom> rooms = hotelroomDict.Values.ToList();// MTR – List 
            int chosenOption = -1;

            do
            {
                HotelRoomsMenu(rooms);
                try
                {
                    int amountRooms = -1;
                    chosenOption = AskMenuOptions();
                    if (chosenOption < 3 && chosenOption < 0)
                    {
                        Console.WriteLine("Input out of range!");
                        Console.ReadLine();
                    }
                    else
                    {
                        switch (chosenOption)
                        {
                            case 1:
                                BookingHotelRoom(rooms);
                                break;

                            case 2:
                                CheckingOutRoom(rooms);
                                break;
                            case 3:
                                HotelroomsNetto(rooms);
                                break;

                            case 0:
                                break;
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.ReadLine();
                }
            } while (chosenOption != 0);
        }
        static void HotelRoomsMenu(List<Hotelroom> rooms)//menu display for hotel rooms
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            PrintLine();
            Console.WriteLine("|\t    Hotelrooms\t\t\t|");
            PrintLine();
            HotelRoomsDisplay(rooms);
            PrintLine();
            Console.WriteLine("|\t1. Book a room (1- 4) people\t|");
            Console.WriteLine("|\t2. Check out of room\t\t|");
            Console.WriteLine("|\t3. Get netto of the hotel\t\t|");
            Console.WriteLine("|\t0. Stop program\t\t\t|");
            PrintLine();
        }
        static void HotelRoomsDisplay(List<Hotelroom> rooms) //methode to dispaly rooms
        {
            // MTR – Foreach
            // foreach to display rooms
            foreach (var room in rooms)
            {
                Console.WriteLine(room.ToString());
            }
        }
        static Dictionary<string, Hotelroom> LoadHotelroomFolder()//methode to load the rooms from their file
        {
            var dictRooms = new Dictionary<string, Hotelroom>();
            string filePath = Path.Combine(rootFolderHotelrooms, "Rooms.txt");

            // Search for .map files in subdirectories 
            foreach (var rooms in File.ReadAllLines(filePath))
            {
                string[] parts = rooms.Split(','); //splitting the parts by comma

                if (!IsValidAHotelroomtFile(rooms))
                {
                    Console.WriteLine($"Invalid room file not added.\nPlease check if file requirements are met");
                    Console.WriteLine("Example of a valid file");
                    Console.WriteLine("1,Basic,X\r\n2,Deluxe,-\r\n3,Comfort,X");
                    Console.WriteLine("etc...");
                    Console.ReadLine();
                    continue;
                }

                //trimming the parts and matching them with hotelroom parameters
                int roomNumber = int.Parse(parts[0].Trim());
                RoomType roomType = Enum.Parse<RoomType>(parts[1].Trim());
                bool occupied = parts[2].Trim().Equals("X", StringComparison.OrdinalIgnoreCase);

                var roomToAdd = new Hotelroom(roomType, roomNumber)
                {
                    occupied = occupied
                };
                dictRooms[roomNumber.ToString()] = roomToAdd;
            }
            return dictRooms;
        }
        static bool IsValidAHotelroomtFile(string rooms)//methode for validating room files to be loaded
        {
            string[] parts = rooms.Split(',');

            if (parts.Length != 3)
                return false;
            // Validate room number
            if (!int.TryParse(parts[0].Trim(), out _))
                return false;

            // Validate room type
            if (!Enum.TryParse(parts[1].Trim(), out RoomType _))
                return false;

            // Validate occupancy character
            string status = parts[2].Trim();
            if (status != "X" && status != "-")
                return false;
            return true;
        }
        static void BookingHotelRoom(List<Hotelroom> rooms)//methode for booking out a hotel room
        {
            try
            {
                if (rooms.All(r => r.occupied))// checking if all room are occuppied
                {
                    Console.WriteLine("All rooms are occupied at the moment!");
                    Console.ReadLine();
                    return;
                }

                Console.Write("Amount of rooms to book: ");
                int amountRooms = int.Parse(Console.ReadLine());

                if (amountRooms > 4 || amountRooms <= 0)
                {
                    Console.WriteLine("Input out of range!");
                    Console.ReadLine();
                    return;
                }

                RoomType[] sorting =
                {  RoomType.Exclusive,RoomType.Deluxe, RoomType.Comfort, RoomType.Basic};

                int bookedCount = 0;

                foreach (var type in sorting) // foreach using sorting to start from exclusive to basic
                {
                    foreach (var room in rooms)
                    {
                        if (!room.occupied && room.GetRoomType() == type)// if not occupied and the type matches with the sorting the occupy 
                        {
                            room.Book(0);
                            bookedCount++;

                            if (bookedCount == amountRooms)
                                break;
                        }
                    }
                    if (bookedCount == amountRooms)
                        break;
                }

                if (bookedCount < amountRooms)
                {
                    Console.WriteLine($"Only {bookedCount} rooms were available to book.");
                    Console.ReadLine();
                }
                SaveHotelRoomsEditsBackToFile();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Try again.");
                Console.ReadLine();
            }
        }
        static void CheckingOutRoom(List<Hotelroom> rooms)//methode for checking out of a room 
        {
            try
            {
                Console.Write("Room to check out of: ");
                int roomNumber = int.Parse(Console.ReadLine());

                if (roomNumber > rooms.Count || roomNumber < 1)
                {
                    Console.WriteLine("Input out of range!");
                    Console.ReadLine();
                }
                else //changing occupation to false by the number given
                {
                    roomNumber = roomNumber - 1;
                    rooms[roomNumber].occupied = false;
                    SaveHotelRoomsEditsBackToFile();
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                Console.ReadLine();
            }
        }
        static void HotelroomsNetto(List<Hotelroom> rooms)
        {
            double rev;
            double cost;
            double totRev = 0;
            foreach (var room in rooms)
            {
                int bookedCount = 0;

                if (!room.occupied)
                {
                    bookedCount++;
                }
                rev = room.GetRevenue() * bookedCount;
                cost = room.GetCost() * rooms.Count;
                totRev = rev - cost;
            }

            Console.WriteLine($"total rev= {totRev}");
            Console.ReadLine();

        }
        static int AskMenuOptions() // methode to get the option that is made in a menu and return it
        {
            Console.Write("Choice > ");
            int choice = int.Parse(Console.ReadLine());
            return choice;
        }
        static void PrintLine() // methode for menu lines for a better UI
        {
            Console.WriteLine("+---------------------------------------+");
        }
        static void MainMenu()//methode for the main menu
        {
            PrintLine();
            Console.WriteLine("|\t        |MENU|\t\t\t|");
            PrintLine();
            Console.WriteLine("|\t  1 - Hotelrooms  \t\t|");
            Console.WriteLine("|\t  2 - Aircrafts   \t\t|");
            Console.WriteLine("|\t  0 - Stop    \t\t\t|");
            PrintLine();

        }
        static void AircraftMenu() //methode for aircrafts menus display
        {
            Console.Clear();

            PrintLine();
            Console.WriteLine("|\t     |AIRCRAFT MENU|\t\t|");
            PrintLine();
            Console.WriteLine("| 1 - Overview of existing aircraft\t|");
            Console.WriteLine("| 2 - Details of an aircraft\t\t|");
            Console.WriteLine("| 3 - Depart or Arrive an aircraft\t|");
            Console.WriteLine("| 4 - Book a seat an aircraft\t\t|");
            Console.WriteLine("| 5 - Get an aircrafts revenue\t\t|");
            Console.WriteLine("| 6 - Get full revenue details\t\t|");
            Console.WriteLine("| 7 - Get revenue of whole airport\t\t|");
            Console.WriteLine("| 0 - Back to main menu\t\t\t|");
            PrintLine();

        }
        static Dictionary<string, Aircraft> LoadAircraftsFromFolder()//methode to load all the files in the aircraft folder
        {
            var dicAircraft = new Dictionary<string, Aircraft>();
            // Search for .map files in subdirectories 
            foreach (var filePath in Directory.GetFiles(rootFolderAircrafts, "*.map", SearchOption.AllDirectories))
            {
                string id = Path.GetFileNameWithoutExtension(filePath);
                string[] aircraftFile = File.ReadAllLines(filePath);

                if (IsValidAircraftFile(aircraftFile, filePath) == false)
                {
                    Console.WriteLine($"Invalid aircraft file not added.\nPlease check if file requirements are met");
                    Console.WriteLine($"{filePath}");
                    Console.WriteLine("Example of a valid file");
                    Console.WriteLine("Airbus H130\r\n-X--\r\n    \r\n---X\r\n");
                    Console.ReadLine();
                    continue;
                }

                string name = aircraftFile[0].Trim();
                var seatLines = aircraftFile.Skip(1).ToList(); // skip name line, keep all seat lines (including gaps)
                char[,] seats = AircraftSeatLayout((seatLines));

                // Creating Aircraft object from the file 
                var aircraft = CreatingAircraftObject(filePath, id, name, seats);
                dicAircraft[id] = aircraft;
                dicAircraft[id] = aircraft;
            }

            return dicAircraft;
        }
        static bool IsValidAircraftFile(string[] seats, string filePath)//methode for validating aircraft files to be loaded
        {
            bool check = false;
            var seatLines = seats.Skip(1);
            if (seats.Length < 1)//checking it has more than 2 (just the name)
            { return check; }
            else if (string.IsNullOrWhiteSpace(seats[0]))
                return check;
            else if (seatLines.All(rows => rows.All(chars => chars == 'X' || chars == '-' || chars == ' ')))//checking the chars match and exists
            { return check = true; }
            return check;
        }
        static Aircraft CreatingAircraftObject(string filePath, string id, string name, char[,] seats)// methode for creating aircraft object
        {
            if (filePath.Contains(Path.DirectorySeparatorChar + "Helicopter" + Path.DirectorySeparatorChar,
                                  StringComparison.OrdinalIgnoreCase)) //if it's from helicopter folder return it as a helicopter object
            {
                bool isMilitary = name.EndsWith(",M", StringComparison.OrdinalIgnoreCase);
                return new Helicopter(id, name, seats, isMilitary);
            }
            else if (filePath.Contains(Path.DirectorySeparatorChar + "Plane" + Path.DirectorySeparatorChar,
                                       StringComparison.OrdinalIgnoreCase))//if it's from plane folder return it as a plane object
            {
                return new Plane(id, name, seats);
            }
            else //default is just an aircraft object
            {
                return new Aircraft(id, name, seats);
            }
        }
        static char[,] AircraftSeatLayout(List<string> seatLines)//seat layout for aircrafts
        {
            int rows = seatLines.Count;
            int cols = seatLines.Select(line => line.TrimEnd().Length).Max();

            char[,] seats = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                string seatRow = seatLines[r];
                for (int c = 0; c < cols; c++)
                {
                    seats[r, c] = c < seatRow.Length ? seatRow[c] : ' ';
                }
            }

            return seats;
        }
        static void AircraftSeatLayout(char[,] seats)//MTR - Function overloading
        {
            int rows = seats.GetLength(0);
            int cols = seats.GetLength(1);

            Console.Write("   ");
            for (int c = 0; c < cols; c++)
                Console.Write($"{c + 1:D2} ");
            Console.WriteLine();

            for (int r = 0; r < rows; r++)
            {
                Console.Write((char)('A' + r) + "  ");
                for (int c = 0; c < cols; c++)
                    Console.Write(seats[r, c] + "  ");
                Console.WriteLine();
            }
        }
        static void Aircrafts()//methode for the logistics of aircrafts
        {
            int chosenOption = -1;
            do
            {
                AircraftMenu();
                try
                {
                    chosenOption = AskMenuOptions();
                    string aircraftId = " ";

                    switch (chosenOption)
                    {
                        case 1: //overview of all aircrafts
                            AllPlanes();
                            break;
                        case 2: // details of a specific aircraft
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            AircraftInfo(aircraftId);
                            break;
                        case 3: // departing or arriving a specific aircraft
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            if (CheckAircraftExists(aircraftId) == true)
                            {
                                Console.WriteLine("Type (D) to depart or (A) to arrive");
                                string depArrChoice = Console.ReadLine();
                                if (depArrChoice.Equals("D"))
                                    AircraftDepart(aircraftId);
                                else if (depArrChoice.Equals("A"))
                                    AircraftArrive(aircraftId);
                            }
                            break;
                        case 4: // booking a seat in a specific aircraft
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            BookAircraftSeat(aircraftId);
                            break;
                        case 5:
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            AircraftNetto(aircraftId);
                            break;
                        case 6:
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            AircraftNettoDetails(aircraftId);
                            break;
                        case 7: //make a methode to get the profits of the whole aircrafts 
                            AllAircraftsIds();
                            Console.Write("Aircraft ID: ");
                            aircraftId = Console.ReadLine();
                            break;
                        case 0:
                            break;
                        default:
                            Console.WriteLine("Input out of range!");
                            Console.ReadLine();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.ReadLine();
                }

            } while (chosenOption != 0);
        }
        static void AllPlanes()//methode to get an overview of all planes
        {
            Console.Clear();
            Console.WriteLine("\t| OVERVIEW OF PLANES |");
            PrintLine();

            if (aircraftDict == null || aircraftDict.Count == 0) //if dictionary is empty
            {
                Console.WriteLine("No aircraft loaded.");
                Console.ReadLine();
                return;
            }
            foreach (var ac in aircraftDict) // foreach to get all the values in the aircraft dictionary
            {
                Aircraft a = ac.Value;
                Console.WriteLine(a.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }
        static void AircraftInfo(string id)//methode to get information over a specific aircraft
        {
            string detailsOfSeats = " ";
            if (CheckAircraftExists(id) == false) { Console.ReadLine(); return; }

            Aircraft aircraft = aircraftDict[id];

            if (aircraft is Helicopter helicopter && helicopter.IsMilitaryGetter())// if its military then it can not be shown
            {
                RedColoredMessage("Military aircraft's details can not be shown.");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(aircraftDict.FirstOrDefault(x => x.Key == id).Value); //getting the information by it's id
                PrintLine();
                Console.WriteLine();
                char[,] seats = aircraft.GetSeats(); // add a getter in Aircraft if seats is private
                // Calculations for the details
                var (amountOfReserved, amountOfNotReserved) = CountSeatStatus(seats);
                int totalSeats = amountOfNotReserved + amountOfReserved;

                double percentage = (amountOfReserved / (double)totalSeats) * 100;
                percentage = Math.Round(percentage);
                AircraftSeatLayout(seats);
                detailsOfSeats = $"\n> Number of available seats: {amountOfNotReserved}" +
                    $"\n> Number of reserved seats: {amountOfReserved}" +
                    $"\n> Occupancy (expressed in %): {percentage}%";
                Console.WriteLine(detailsOfSeats);
                Console.WriteLine();
                if (aircraft.GetIsAirborne() == true)
                {
                    RedColoredMessage("This aircraft has departed!");
                }
                else if (amountOfNotReserved == 0)
                {
                    RedColoredMessage("This aircraft no more seats available!");
                }
                Console.WriteLine("Press Enter to return to the menu...");
            }
            Console.ReadLine();
        }
        static void AllAircraftsIds()//methode to get id's all planes
        {
            foreach (var id in aircraftDict.Keys)
            {
                Console.Write(id + " | ");
            }
            Console.WriteLine();
        }
        static void AircraftDepart(string id)//methode to make an specific aicraft depart
        {
            Aircraft aircraft = aircraftDict[id];
            char[,] seats = aircraft.GetSeats();
            int amountOfReserved = CountingStatus(seats, 'X');
            CheckAircraftExists(id);
            if (amountOfReserved <= 0) // if no one is inside then it cant depart
            {
                Console.WriteLine("An empty aircraft can not be departed");
                Console.WriteLine(amountOfReserved);
            }
            else if (aircraft.GetIsAirborne() == true) // checking if its already departed
            {
                Console.WriteLine("This Aircraft has already departed");
            }
            else
            {
                aircraft.Depart();
                Console.WriteLine("Aircraft has successful departed!");
            }
            Console.ReadLine();
        }
        static void AircraftArrive(string id)//methode to make an aircraft (given by id) arrive
        {
            Aircraft aircraft = aircraftDict[id];
            CheckAircraftExists(id);
            if (aircraft.GetIsAirborne() == false) //checking if it already arrived
            {
                Console.WriteLine("This plane has already arrived");
            }
            else
            {
                aircraft.Arrive();
                SaveAircraftEditsBackToFile();
                Console.WriteLine("Aircraft has successful arrived!");
            }
            Console.ReadLine();
        }
        static void BookAircraftSeat(string id)//methode to cook a seat in an aircraft
        {
            Aircraft aircraft = aircraftDict[id];
            const int amount = 1;  // MTR -  Constant

            if (!CheckAircraftExists(id)) { Console.ReadLine(); return; }

            else if (aircraft is Helicopter helicopter && helicopter.IsMilitaryGetter())// checking if it's military meaning it can't be departed
            {
                Console.WriteLine("This aircraft is military and can not be booked.");
            }
            else
            {
                if (aircraft.GetIsAirborne() == true) //checking if it has already departed
                {
                    Console.WriteLine("Aircraft can not booked when already departed!");
                }
                else if (!aircraft.isBookable(amount)) //checking if seats are available
                {
                    Console.WriteLine("There are no more available seats in this aircrafts!");
                }
                else if (aircraft.isBookable(amount)) //when bookable the, you can book
                {
                    aircraft.Book(amount);
                    Console.WriteLine($"Seat in {id} has been successfully booked.");
                    SaveAircraftEditsBackToFile();
                }
                Console.ReadLine();
            }
        }
        static bool CheckAircraftExists(string id)//methode to check the existance of an aircraft given by aircraft id
        {
            if (!aircraftDict.ContainsKey(id)) //checking if the key is in the aricraftdictionary
            {
                Console.WriteLine("Aircraft given doesn't exist.");
                return false;
            }
            return true;
        }
        static int CountingStatus(char[,] seats, char status)//methode to count reserved or not reserved seats  | MTR - Function overloading
        {
            int count = 0;
            foreach (char seat in seats) // foreach counts every time the seat matches the char given
            {
                if (seat == status) count++;
            }
            return count;
        }
        static (int reserved, int available) CountSeatStatus(char[,] seats)//methode to count reserved and not reserved seats  | MTR Tuples
        {
            int reserved = 0, notReserved = 0;
            foreach (char seat in seats) // when both char are matched they get counted by their int
            {
                if (seat == 'X') reserved++;
                else if (seat == '-') notReserved++;
            }
            return (reserved, notReserved);
        }
        static void AircraftNetto(string id)
        {
            double netto = 0;
            if (!CheckAircraftExists(id)) { Console.ReadLine(); return; }
            Aircraft aircraft = aircraftDict[id];
            if (aircraft is Helicopter helicopter && helicopter.IsMilitaryGetter())// if its military then it can not be shown
            {
                RedColoredMessage("Can not show military aircraft revenue!");
                Console.ReadLine();
                return;
            }
            else
            {
                double amountOfSeats = CountingStatus(aircraft.GetSeats(), 'X');
                double cost = aircraft.GetRevenue() * amountOfSeats;
                netto = cost - aircraft.GetCost();
                Console.WriteLine($"The netto of aircraft {id} is {netto}");
                Console.WriteLine($"Profit = {netto - aircraft.GetCost()}");
            }
            Console.ReadLine();
        }
        static void AircraftNettoDetails(string id)
        {
            double netto = 0;
            if (!CheckAircraftExists(id)) { Console.ReadLine(); return; }
            Aircraft aircraft = aircraftDict[id];
            if (aircraft is Helicopter helicopter && helicopter.IsMilitaryGetter())// if its military then it can not be shown
            {
                List<double> rev = new List<double> { };
                foreach (var item in aircraftDict)
                {
                    Aircraft allCrafts = item.Value;
                    double amountOfSeats = CountingStatus(allCrafts.GetSeats(), 'X');
                    double cost = allCrafts.GetRevenue() * amountOfSeats;
                    netto = cost - allCrafts.GetCost();
                    rev.Add(netto);

                }
                Console.WriteLine($"Total revenue of all planes = {rev.Sum()}");
                Console.ReadLine();
                return;
            }
            else
            {
                double amountOfSeats = CountingStatus(aircraft.GetSeats(), 'X');
                double cost = aircraft.GetRevenue() * amountOfSeats;
                netto = cost - aircraft.GetCost();
                Console.WriteLine($"Total netto = {cost}");
                Console.WriteLine($"Total cost = {aircraft.GetCost()}");
                Console.WriteLine($"Netto profit = {netto}");
            }
            Console.ReadLine();
        }
        static void SaveAircraftEditsBackToFile()//methode to save updates made to aircrafts seats
        {
            foreach (var filePath in Directory.GetFiles(rootFolderAircrafts, "*.map", SearchOption.AllDirectories))
            {
                string id = Path.GetFileNameWithoutExtension(filePath);

                var aircraft = aircraftDict[id];

                // Keep original first line (name)
                var detailsLine = File.ReadAllLines(filePath).ToList();
                if (detailsLine.Count == 0) continue;

                char[,] seats = aircraft.GetSeats();
                int rows = seats.GetLength(0);
                int cols = seats.GetLength(1);

                var updatedSeats = new List<string> { detailsLine[0] };

                for (int r = 0; r < rows; r++)
                {
                    var rowChars = new char[cols];
                    for (int c = 0; c < cols; c++)
                    {
                        rowChars[c] = seats[r, c];
                    }
                    updatedSeats.Add(new string(rowChars));
                }

                File.WriteAllLines(filePath, updatedSeats);//rewrite the file with new updated seats
            }
        }
        static void SaveHotelRoomsEditsBackToFile() //methode to save update mades to rooms
        {
            string filePath = Path.Combine(rootFolderHotelrooms, "Rooms.txt"); // getting the file

            var updatedRooms = hotelroomDict //getting data from hoteldict 
                .OrderBy(roomsDict => int.Parse(roomsDict.Key))
                .Select(roomsDict => $"{roomsDict.Key},{roomsDict.Value.GetRoomType()},{(roomsDict.Value.occupied ? "X" : "-")}");

            File.WriteAllLines(filePath, updatedRooms); //rewritting the file with the new data
        }
        static void RedColoredMessage(string message) //methode to change the color of a string to red ref:https://stackoverflow.com/questions/2743260/is-it-possible-to-write-to-the-console-in-colour-in-net
        {
            string RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
            string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";

            Console.WriteLine($"{RED}{message}{NORMAL}");
        }
    }

}
