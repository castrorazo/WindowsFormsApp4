using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp4;

class SerialPortReader
{
    static Form1 mainForm;  // Keep a reference to the main form

    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        mainForm = new Form1();

        // Configure and start reading from the serial port
        SerialPort serialPort = ConfigureSerialPort();
        Task.Run(() => ReadFromSerialPort(serialPort));

        Application.Run(mainForm);
    }

    static SerialPort ConfigureSerialPort()
    {
        SerialPort serialPort = new SerialPort
        {
            PortName = "COM4",
            BaudRate = 115200,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            ReadTimeout = 3000
        };

        return serialPort;
    }

    static void ReadFromSerialPort(SerialPort serialPort)
    {
        try
        {
            serialPort.Open();
            Console.WriteLine("Port opened. Listening for data...");

            while (true)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        string message = serialPort.ReadLine();
                        Console.WriteLine("Received: " + message);
                        mainForm.UpdateLabelText(message);
                    }
                }
                catch (TimeoutException)
                {
                    // Handle timeout situation, if necessary
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Port closed.");
            }
        }
    }
}