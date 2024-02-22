using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ejemplo_Elster
{
    public class LibreriaElster
    {

        [DllImport("LibreriaElster.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GeneraLLave")]
        public static extern void GeneraLlave(int[] CadenaE);

        static SerialPort spMc = new SerialPort();
        static SerialPort spMe = new SerialPort();

        public LibreriaElster()
        {
            timer.Elapsed += new ElapsedEventHandler(activaTimeOut);
            timer.Interval = 25000;

            timer.Elapsed += new ElapsedEventHandler(activaTimeOut2);
            timer.Interval = 5000;

            timer.Enabled = true;
            timer2.Enabled = true;
        }

        static System.Timers.Timer timer = new System.Timers.Timer();
        static System.Timers.Timer timer2 = new System.Timers.Timer();
        static bool timerTimeOut;
        /*
        byte[] saludo1 = new byte[] { 0x55, 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        byte[] saludo2 = new byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        byte[] saludo3 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x05, 0xb0, 0x40 };
        byte[] saludo4 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x05, 0xb0, 0x40 };


        byte[] comun1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x20, 0x62, 0x3a };
        byte[] comun2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xe0, 0x00, 0x00, 0x00, 0x00, 0x01, 0xfa, 0xaa };
        byte[] comun3 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xc3, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x91, 0xcb };
        byte[] comun4 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 };
        byte[] comun5 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x45, 0x99 };

        byte[] lectura = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x29, 0x82, 0x0b };// LECTURA

        byte[] corte1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x00, 0x00, 0xf2, 0x88 };//corte
        byte[] corte2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 };//corte

        byte[] reconexion1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x80, 0x00, 0x3e, 0x04 };//conexion
        byte[] reconexion2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 };//conexion
        */

        //COMUN
        Byte[] tramaGM1 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        Byte[] tramaGM2 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x05, 0x71, 0x3C, 0x03, 0x1E, 0x00, 0xB7, 0x3D };
        Byte[] tramaGM3 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0x26, 0xEE, };
        Byte[] tramaGM4 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x0D, 0x50, 0x27, 0x10, 0x61, 0x33, 0x6C, 0x69, 0x6D, 0x20, 0x20, 0x20, 0x20, 0x20, 0xC3, 0x84 };

        //LECTURA
        Byte[] tramaGM5 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x01, 0x55, 0x0D };
        Byte[] tramaGM6 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x01, 0x20, 0xD4, 0x78 };

        //METERCAT
        Byte[] tramaGMe1 = new Byte[] { 0x06, 0xEE, 0xD0, 0x20, 0x00, 0x00, 0x05, 0x00, 0x04, 0x00, 0xFF, 0x00, 0xD0, 0xB4 };
        Byte[] tramaGMe2 = new Byte[] { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x01, 0x00, 0xD2, 0x74 };
        Byte[] tramaGMe3 = new Byte[] { 0x06, 0xEE, 0xD0, 0x20, 0x00, 0x00, 0x24, 0x00, 0x00, 0x20, 0x45, 0x45, 0x20, 0x20, 0x41, 0x33, 0x52, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0x03, 0x03, 0x06, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x31, 0x32, 0x39, 0x31, 0x38, 0x37, 0x36, 0x33, 0x1D, 0x1E, 0x92 };
        Byte[] tramaGMe4 = new Byte[] { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x14, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x31, 0x32, 0x39, 0x31, 0x38, 0x37, 0x36, 0x33, 0xDB, 0x3B, 0x52 };
        Byte[] tramaGMe5 = new Byte[] { 0x06, 0xEE, 0xD0, 0x20, 0x00, 0x00, 0x18, 0x00, 0x00, 0x14, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x33, 0x35, 0x54, 0x31, 0x31, 0x47, 0xDB, 0x4F, 0xC5 };
        Byte[] tramaGMe6 = new Byte[] { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x20, 0x31, 0x36, 0x00, 0x02, 0x50, 0x02, 0x03, 0x30, 0x4B, 0x00, 0x02, 0x60, 0x05, 0x02, 0x00, 0x02, 0x31, 0x4E, 0x53, 0xDF, 0x01, 0x00, 0x03, 0x30, 0x31, 0x00, 0x30, 0x31, 0x00, 0x01, 0x3F, 0x00, 0xA5, 0x57, 0x19 };

        //LECTURA
        Byte[] tramaM1 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x05, 0x71, 0x3C, 0x03, 0x1E, 0x00, 0xC0, 0x92 };
        Byte[] tramaM2 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0x2C, 0xC3 };
        Byte[] tramaM3 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0D, 0x50, 0x00, 0x00, 0x61, 0x33, 0x6C, 0x69, 0x6D, 0x20, 0x20, 0x20, 0x20, 0x20, 0xB3, 0xB4};
        //                                                                                                                          Lan Id 15, 16, 17, 18
        Byte[] tramaM5 = new Byte[] {0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0D, 0x40, 0x00, 0x07, 0x00, 0x07, 0x08, 0x30, 0x00, 0x00, 0xA7, 0xB8, 0xB7, 0xB2, 0xC8, 0x5A };

        Byte[] tramaM6 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        //                                                                                                                                                                                      Lan Id 25,26, 27, 28
        Byte[] tramaM7 = new Byte[] {0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x17, 0x4F, 0x08, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x58, 0x97, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0xFA, 0x08, 0x4A };

        Byte[] tramaM8 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0xC1, 0x75, 0x6D };
        Byte[] tramaM9 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x00,  0xB0, 0xE9, 0x7C};
        Byte[] tramaM10 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] tramaM11 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x01, 0x52, 0xD0, 0x48 };
        Byte[] tramaM12 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x01, 0x21, 0x5D, 0x69 };

        //CORTE

//        Byte[] corte1 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        Byte[] corte2 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x05, 0x71, 0x3C, 0x03, 0x1E, 0x00, 0xB7, 0x3D };
        Byte[] corte3 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0x26, 0xEE };
        Byte[] corte4 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x0D, 0x50, 0x27, 0x10, 0x61, 0x33, 0x6C, 0x69, 0x6D, 0x20, 0x20, 0x20, 0x20, 0x20, 0xC3, 0x84 };
        Byte[] corte5 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x13, 0x00, 0x01, 0xE2, 0x70 };
        Byte[] corte6 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x08, 0x3F, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x20, 0x03, 0x05 };
        Byte[] corte7 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x99, 0xE1 };
        Byte[] corte8 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x05, 0xF2, 0x28 };
        Byte[] corte9 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x53, 0x09, 0x00, 0x83, 0x81, 0xB3, 0x83, 0x89, 0x0F, 0x03, 0xFD, 0x53, 0x4E };
        Byte[] corte10 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0xF9, 0x64 };
        Byte[] corte11 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3F, 0x08, 0x08, 0x00, 0x01, 0x83, 0x00, 0x06, 0x2B, 0x6E };
        Byte[] corte12 = new Byte[] { 0x06, 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x37, 0x63, 0x3a };
        Byte[] corte13 = new Byte[] { 0x06, 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x5C, 0x35, 0x84 };
        Byte[] corte14 = new Byte[] { 0x06, 0xEE, 0xD0, 0x20, 0x00, 0x00, 0x01, 0x52, 0xD4, 0x65 };
        Byte[] corte15 = new Byte[] { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x01, 0x21, 0x59, 0x44 };
        Byte[] corte16 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x01, 0x20, 0x45, 0x18 };
        Byte[] corte17 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x05, 0x71, 0x3C, 0x03, 0x1E, 0x00, 0xF9, 0x65 };
        Byte[] corte18 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0xDC, 0x75 };
        Byte[] corte19 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x0D, 0x50, 0x00, 0x00, 0x61, 0x33, 0x6C, 0x69, 0x6D, 0x20, 0x20, 0x20, 0x20, 0x20, 0x71, 0xA5 };
        Byte[] corte20 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0B, 0x53, 0x09, 0x00, 0x34, 0x68, 0x2A, 0x1D, 0x78, 0x9B, 0x23, 0x21, 0x31, 0x00 };
        Byte[] corte21 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x08, 0xB8, 0x36 };
        Byte[] corte22 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte23 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x9D, 0x9C, 0xF5 };
        Byte[] corte24 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x01, 0xAF, 0x47, 0x8D };
        Byte[] corte25 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte26 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x9B, 0x29, 0xF3 };
        Byte[] corte27 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x01, 0xB9, 0x65 };
        Byte[] corte28 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x03, 0xE8, 0xEB };
        Byte[] corte29 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0xD2, 0x6F, 0x4F };
        Byte[] corte30 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x04, 0x60, 0x01, 0x00, 0x80, 0x61, 0x4C };
        Byte[] corte31 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x2B, 0x4F, 0x08, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x22, 0x58, 0x97, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0xA7, 0xB8, 0xF6, 0x00, 0xA3, 0xB8, 0x20, 0x00, 0x84, 0xBB, 0xB1, 0x00, 0x84, 0xB8, 0x38, 0x00, 0x84, 0xB8, 0x39, 0x00, 0x84, 0xB1,0xC5, 0x68, 0x5C, 0xCC };
        Byte[] corte32 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0xDC, 0x75 };
        Byte[] corte33 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x08, 0x3F, 0x08, 0xC1, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x9E, 0x59 };
        Byte[] corte34 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0xC1, 0xF6, 0x0E };
        Byte[] corte35 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte36 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte37 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte38 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte39 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte40 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte41 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte42 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte43 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte44 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x04, 0x60, 0x01, 0x00, 0x80, 0x61, 0x4C };
        Byte[] corte45 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x23, 0x4F, 0x08, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x1A, 0x58, 0xFB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x84, 0xBB, 0xB1, 0x00, 0x84, 0xB8, 0x38, 0x00, 0x84, 0xB8, 0x39, 0x00, 0x84, 0xB1, 0xC5, 0xD6, 0x33, 0x92 };
        Byte[] corte46 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x04, 0x60, 0x04, 0x00, 0x80, 0xDC, 0x75 };
        Byte[] corte47 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x08, 0x3F, 0x08, 0xC1, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x9E, 0x59 };
        Byte[] corte48 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0xC1, 0xF6, 0x0E };
        Byte[] corte49 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte50 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte51 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte52 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte53 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte54 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte55 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte56 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte57 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x42, 0x53 };
        Byte[] corte58 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x9C, 0x96, 0x87 };
        Byte[] corte59 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0xAD, 0x1F, 0xC4 };
        Byte[] corte60 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x02, 0x70, 0xFF, 0x22, 0xD6 };
        Byte[] corte61 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x9B, 0xAA, 0x90 };
        Byte[] corte62 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x96, 0xCC, 0x28 };
        Byte[] corte63 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x9E, 0x07, 0xC7 };
        Byte[] corte64 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x1A, 0x40, 0x00, 0x07, 0x00, 0x14, 0x08, 0x36, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0x40, 0x00, 0x07, 0x00, 0x03, 0x08, 0xC9, 0x02, 0x00, 0x00, 0x00, 0x8E, 0xDC, 0xE4 };
        Byte[] corte65 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte66 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x1A, 0x40, 0x00, 0x07, 0x00, 0x14, 0x08, 0x36, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0x40, 0x00, 0x07, 0x00, 0x03, 0x08, 0xC9, 0x02, 0x80, 0x00, 0x00, 0x0E, 0xBA, 0x4D };
        Byte[] corte67 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte68 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x1A, 0x40, 0x00, 0x07, 0x00, 0x14, 0x08, 0x36, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0x40, 0x00, 0x07, 0x00, 0x03, 0x08, 0xC9, 0x02, 0x00, 0x00, 0x00, 0x8E, 0xDC, 0xE4 };
        Byte[] corte69 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte70 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0D, 0x40, 0x00, 0x07, 0x00, 0x07, 0x08, 0x30, 0x00, 0x00, 0xA7, 0xB8, 0xB7, 0xB2, 0xC8, 0x5A };
        Byte[] corte71 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte72 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x17, 0x4F, 0x08, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x58, 0x97, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0xFA, 0x08, 0x4A };
        Byte[] corte73 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0xC1, 0x75, 0x6D };
        Byte[] corte74 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x00, 0xB0, 0xE9, 0x7C };
        Byte[] corte75 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte76 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x01, 0xAF, 0x47, 0x8D };
        Byte[] corte77 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte78 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x00, 0xB0, 0xE9, 0x7C };
        Byte[] corte79 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte80 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x01, 0xAF, 0x47, 0x8D };
        Byte[] corte81 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte82 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x48, 0x00, 0x00, 0xB0, 0xE9, 0x7C };
        Byte[] corte83 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x78, 0xF8 };
        Byte[] corte84 = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x01, 0x52, 0xD0, 0x48 };
        Byte[] corte85 = new Byte[] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x01, 0x21, 0x5D, 0x69 };

        Byte[] reco = new Byte[] { 0x06, 0xEE, 0x02, 0x00, 0x00, 0x00, 0x1A, 0x40, 0x00, 0x07, 0x00, 0x14, 0x08, 0x36, 0x00, 0x00, 0x01, 0x00, 0xA7, 0xB8, 0xB7, 0x40, 0x00, 0x07, 0x00, 0x03, 0x08, 0xC9, 0x02, 0x80, 0x80, 0x00, 0x8E, 0xBA, 0x4D };

        public String EjecutaTareaElster(int tarea, String portName, byte []lan)
        {
            String respuesta = "";

            if (tarea == 1)
            {
                List<Byte[]> tramasGM = new List<Byte[]>();
                tramasGM.Add(tramaGM1);
                tramasGM.Add(tramaGM2);
                tramasGM.Add(tramaGM3);
                tramasGM.Add(tramaGM4);
                tramasGM.Add(tramaGM5);
                tramasGM.Add(tramaGM6);

                List<Byte[]> tramasGMe = new List<Byte[]>();
                tramasGMe.Add(tramaGMe1);
                tramasGMe.Add(tramaGMe2);
                tramasGMe.Add(tramaGMe3);
                tramasGMe.Add(tramaGMe4);
                tramasGMe.Add(tramaGMe5);
                tramasGMe.Add(tramaGMe6);

                List<Byte[]> tramaM = new List<byte[]>();

                

                try
                {
                    spMe.Parity = Parity.None;
                    spMe.StopBits = StopBits.One;
                    spMe.DataBits = 8;
                    spMe.PortName = portName;

                    spMc.Parity = Parity.None;
                    spMc.StopBits = StopBits.One;
                    spMc.DataBits = 8;
                    spMc.PortName = "COM21";

                    inicializaPuerto(0, portName, spMe);
                    spMc.Open();
                    spMe.Open();
                    int i = 0;
                    Byte[] fKey = null;

                    Thread.Sleep(350);

                    foreach (Byte[] trama in tramasGM)
                    {
                        spMe.Write(trama, 0, trama.Length);

                        byte[] respuestaSerial = null;
                        byte[] auxiliar = null;

                        Thread.Sleep(600);
                        while (spMe.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[spMe.BytesToRead];
                                spMe.Read(respuestaSerial, 0, spMe.BytesToRead);
                                Thread.Sleep(50);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                respuestaSerial = new byte[auxiliar.Length + spMe.BytesToRead];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                byte[] bytes = new byte[spMe.BytesToRead];
                                spMe.Read(bytes, 0, spMe.BytesToRead);
                                Thread.Sleep(50);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }

                        Thread.Sleep(300);

                        if (respuestaSerial != null && i == 5)
                        {
                            fKey = new Byte[8];
                            Array.Copy(respuestaSerial, 15, fKey, 0, 8);
                            spMe.Write(System.Text.Encoding.UTF8.GetString(new Byte[] { 0x06 }));
                        }
                        i += 1;
                    }

//                    timer.Start();
//                    timer2.Start();
                    timerTimeOut = false;

                    if (fKey != null)
                    {
/*
                        bool MeterPc = false;                        
                        while (!timerTimeOut)
                        {
                            if (spMc.BytesToRead > 0)
                            {
                                timer.Stop();
                                Thread.Sleep(600);
                                timerTimeOut = true;
                                MeterPc = true;
                                spMc.ReadExisting();
                            }
                        }

                        if (MeterPc == true)
                        {
                            Byte[] ArrayCRC = new Byte[26];
                            Byte[] FArray = { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x11, 0x00, 0x02, 0x01, 0x00, 0x02, 0x01, 0x00, 0x08 };
                            Byte[] ChkCRC = new Byte[23];

                            Array.Copy(FArray, 1, ChkCRC, 0, 14);
                            Array.Copy(fKey, 0, ChkCRC, 14, 8);

                            Crc16Ccitt crc = new Crc16Ccitt();
                            ushort a = crc.CCITT_CRC16(ChkCRC);
                            Byte[] aCrc = BitConverter.GetBytes(a);
                            Array.Reverse(aCrc);
                            Array.Copy(FArray, 0, ArrayCRC, 0, 15);
                            Array.Copy(fKey, 0, ArrayCRC, 15, 8);
                            Array.Copy(aCrc, 0, ArrayCRC, 24, 2);

                            spMc.Write(ArrayCRC, 0, ArrayCRC.Length);

                        }

                        i = 0;
*/
                        foreach (Byte[] trama in tramasGMe)
                        {
/*
                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            Thread.Sleep(600);

                            while (spMc.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[spMc.BytesToRead];
                                    spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                    Thread.Sleep(50);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[spMc.BytesToRead];
                                    spMc.Read(bytes, 0, spMc.BytesToRead);
                                    Thread.Sleep(50);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }

                            Thread.Sleep(300);

                            spMc.Write(trama, 0, trama.Length);

                            if (i == 5)
                            {
                                Thread.Sleep(600);

                                respuestaSerial = null;
                                auxiliar = null;

                                while (spMc.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[spMc.BytesToRead];
                                        spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        byte[] bytes = new byte[spMc.BytesToRead];
                                        spMc.Read(bytes, 0, spMc.BytesToRead);
                                        Thread.Sleep(50);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
*/            
                                byte[] respuestaKey = EntregaLLave(fKey);

                                Byte[] tramaM4 = new Byte[20] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x0B, 0x53, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                Byte[] ChkCRC = new Byte[17];
                                Array.Copy(respuestaKey, 0, tramaM4, 10, 8);
                                Array.Copy(tramaM4, 1, ChkCRC, 0, 17);
                                Crc16Ccitt crc = new Crc16Ccitt();
                                ushort a = crc.CCITT_CRC16(ChkCRC);
                                Byte[] aCrc = BitConverter.GetBytes(a);
                                Array.Reverse(aCrc);
                                Array.Copy(aCrc, 0, tramaM4, 18, 2);

                                Array.Copy(lan, 0, tramaM5, 15, 4);
                                Byte[] ChkCRC1 = new Byte[19];
                                Array.Copy(tramaM5, 1, ChkCRC1, 0, 19);
                                ushort a1 = crc.CCITT_CRC16(ChkCRC1);
                                Byte[] aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, tramaM5, 20, 2);


                                Array.Copy(lan, 0, tramaM7, 25, 4);
                                Byte[] ChkCRC2 = new Byte[29];
                                Array.Copy(tramaM7, 1, ChkCRC2, 0, 29);
                                ushort a2 = crc.CCITT_CRC16(ChkCRC2);
                                Byte[] aCrc2 = BitConverter.GetBytes(a2);
                                Array.Reverse(aCrc2);
                                Array.Copy(aCrc2, 0, tramaM7, 30, 2);


                                tramaM.Add(tramaM1);
                                tramaM.Add(tramaM2);
                                tramaM.Add(tramaM3);
                                tramaM.Add(tramaM4);
                                tramaM.Add(tramaM5);
                                tramaM.Add(tramaM6);
                                tramaM.Add(tramaM7);
                                tramaM.Add(tramaM8);
                                tramaM.Add(tramaM9);
                                tramaM.Add(tramaM10);
                                tramaM.Add(tramaM11);
                                tramaM.Add(tramaM12);
/*
                            }
                            i += 1;
*/
                        }
                        i = 0;

//                        spMe.Write("AA");
                        Thread.Sleep(350);

                        foreach (Byte[] tramM in tramaM)
                        {
//                            timer2.Stop();
                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            spMe.Write(tramM, 0, tramM.Length);

                            Thread.Sleep(1000);
                            while (spMe.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[spMe.BytesToRead];
                                    spMe.Read(respuestaSerial, 0, spMe.BytesToRead);
                                    Thread.Sleep(50);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + spMe.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[spMe.BytesToRead];
                                    spMe.Read(bytes, 0, spMe.BytesToRead);
                                    Thread.Sleep(50);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                                Thread.Sleep(100);
                            }
                            Thread.Sleep(600);

                            if (i == 7)
                            {
                                String stringBytes = BitConverter.ToString(respuestaSerial);
                                respuesta =  "-";
                                respuesta = respuesta + stringBytes.Split('-')[33] + stringBytes.Split('-')[34] + stringBytes.Split('-')[35] + " kWh";
                            }

                            i += 1;
                        }
                    }

                    spMc.Close();
                    spMe.Close();
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    /*
                                    sp.Write("E");
                                    Thread.Sleep(600);

                                    sp.Write(saludo1, 0, saludo1.Length);

                                    Thread.Sleep(2000);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(saludo2, 0, saludo2.Length);

                                    Thread.Sleep(2000);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    inicializaPuerto(1, portName);
                                    sp.Write(saludo3, 0, saludo3.Length);

                                    Thread.Sleep(2000);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(saludo4, 0, saludo4.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(comun1, 0, comun1.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.Write(lectura, 0, lectura.Length); //lectura
                                        byte[] respuestaSerial = null;
                                        byte[] auxiliar = null;

                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            if (respuestaSerial == null)
                                            {
                                                respuestaSerial = new byte[sp.BytesToRead];
                                                sp.Read(respuestaSerial, 0, sp.BytesToRead);
                                                Thread.Sleep(5);
                                            }
                                            else
                                            {
                                                auxiliar = new byte[respuestaSerial.Length];
                                                auxiliar = respuestaSerial;
                                                respuestaSerial = new byte[auxiliar.Length + sp.BytesToRead];
                                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                                byte[] bytes = new byte[sp.BytesToRead];
                                                sp.Read(bytes, 0, sp.BytesToRead);
                                                Thread.Sleep(5);
                                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                            }
                                        }

                                        if (respuestaSerial != null)
                                        {
                                            respuesta = Encoding.ASCII.GetString(respuestaSerial, 26, 6) + "-";
                                        }
                                        else
                                            respuesta = "";
                                    }

                                    sp.Write(comun2, 0, comun2.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(comun3, 0, comun3.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(comun4, 0, comun4.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    sp.Write(comun5, 0, comun5.Length);

                                    Thread.Sleep(600);
                                    while (sp.BytesToRead > 0)
                                    {
                                        sp.ReadByte();
                                        Thread.Sleep(5);
                                    }

                                    if (tarea == 1)
                                    {
                                        sp.Write(lectura, 0, lectura.Length); //lectura
                                        byte[] respuestaSerial = null;
                                        byte[] auxiliar = null;

                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            if (respuestaSerial == null)
                                            {
                                                respuestaSerial = new byte[sp.BytesToRead];
                                                sp.Read(respuestaSerial, 0, sp.BytesToRead);
                                                Thread.Sleep(5);
                                            }
                                            else
                                            {
                                                auxiliar = new byte[respuestaSerial.Length];
                                                auxiliar = respuestaSerial;
                                                respuestaSerial = new byte[auxiliar.Length + sp.BytesToRead];
                                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                                byte[] bytes = new byte[sp.BytesToRead];
                                                sp.Read(bytes, 0, sp.BytesToRead);
                                                Thread.Sleep(5);
                                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                            }
                                        }

                                        if (respuestaSerial != null)
                                        {
                                            String stringBytes = BitConverter.ToString(respuestaSerial);
                                            respuesta = respuesta + stringBytes.Split('-')[14] + stringBytes.Split('-')[15] + stringBytes.Split('-')[16] + " kWh";
                                        }
                                        else
                                            respuesta = respuesta + "";
                                    }
                                    else if (tarea == 2)
                                    {
                                        sp.Write(corte1, 0, corte1.Length);//corte
                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            sp.ReadByte();
                                            Thread.Sleep(5);

                                        }

                                        sp.Write(corte2, 0, corte2.Length);//corte
                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            sp.ReadByte();
                                            Thread.Sleep(5);

                                        }

                                        respuesta = respuesta + "OK";
                                    }
                                    else if (tarea == 3)
                                    {
                                        sp.Write(reconexion1, 0, reconexion1.Length);//reconexion
                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            sp.ReadByte();
                                            Thread.Sleep(5);
                                        }

                                        sp.Write(reconexion2, 0, reconexion2.Length);//reconexion
                                        Thread.Sleep(600);
                                        while (sp.BytesToRead > 0)
                                        {
                                            sp.ReadByte();
                                            Thread.Sleep(5);
                                        }

                                        respuesta = respuesta + "OK";
                                    }


                                    sp.Close();
                                                    */
                }
                catch (Exception ex)
                {
                    spMc.Close();
                    spMe.Close();
                }
            }
            if (tarea == 2 || tarea == 3)
            {
                List<Byte[]> tramasGM = new List<Byte[]>();
                tramasGM.Add(tramaGM1);

                List<Byte[]> tramasGMe = new List<Byte[]>();
                tramasGMe.Add(tramaGMe1);
                tramasGMe.Add(tramaGMe2);
                tramasGMe.Add(tramaGMe3);
                tramasGMe.Add(tramaGMe4);
                tramasGMe.Add(tramaGMe5);
                tramasGMe.Add(tramaGMe6);

                try
                {
                    spMe.Parity = Parity.None;
                    spMe.StopBits = StopBits.One;
                    spMe.DataBits = 8;
                    spMe.PortName = portName;

                    spMc.Parity = Parity.None;
                    spMc.StopBits = StopBits.One;
                    spMc.DataBits = 8;
                    spMc.PortName = "COM21";

                    inicializaPuerto(0, portName, spMe);
                    spMe.Open();
                    spMc.Open();
                    int i = 0;
                    Byte[] fKey = null;

                    foreach (Byte[] trama in tramasGM)
                    {

                        spMe.Write("AA");
                        Thread.Sleep(350);

                        spMe.Write(trama, 0, trama.Length);

                        byte[] respuestaSerial = null;
                        byte[] auxiliar = null;

                        Thread.Sleep(600);
                        while (spMe.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[spMe.BytesToRead];
                                spMe.Read(respuestaSerial, 0, spMe.BytesToRead);
                                Thread.Sleep(5);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                respuestaSerial = new byte[auxiliar.Length + spMe.BytesToRead];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                byte[] bytes = new byte[spMe.BytesToRead];
                                spMe.Read(bytes, 0, spMe.BytesToRead);
                                Thread.Sleep(5    );
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }

                        Thread.Sleep(300);

                        fKey = new Byte[8];
                        Array.Copy(respuestaSerial, 15, fKey, 0, 8);
                        spMe.Write(System.Text.Encoding.UTF8.GetString(new Byte[] { 0x06 }));
                    }

//                    timer.Start();
//                    timer2.Start();
                    timerTimeOut = false;

                    if (fKey != null)
                    {
                        bool MeterPc = false;
                        while (!timerTimeOut)
                        {
                            if (spMc.BytesToRead > 0)
                            {
                                timer.Stop();
                                Thread.Sleep(600);
                                timerTimeOut = true;
                                MeterPc = true;
                                spMc.ReadExisting();
                            }
                        }

                        if (MeterPc == true)
                        {
                            Byte[] ArrayCRC = new Byte[26];
                            Byte[] FArray = { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x11, 0x00, 0x02, 0x01, 0x00, 0x02, 0x01, 0x00, 0x08 };
                            Byte[] ChkCRC = new Byte[23];

                            Array.Copy(FArray, 1, ChkCRC, 0, 14);
                            Array.Copy(fKey, 0, ChkCRC, 14, 8);

                            Crc16Ccitt crc = new Crc16Ccitt();
                            ushort a = crc.CCITT_CRC16(ChkCRC);
                            Byte[] aCrc = BitConverter.GetBytes(a);
                            Array.Reverse(aCrc);
                            Array.Copy(FArray, 0, ArrayCRC, 0, 15);
                            Array.Copy(fKey, 0, ArrayCRC, 15, 8);
                            Array.Copy(aCrc, 0, ArrayCRC, 24, 2);

                            spMc.Write(ArrayCRC, 0, ArrayCRC.Length);

                        }

                        i = 0;
                        foreach (Byte[] trama in tramasGMe)
                        {
                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            Thread.Sleep(600);
                            while (spMc.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[spMc.BytesToRead];
                                    spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                    Thread.Sleep(50);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[spMc.BytesToRead];
                                    spMc.Read(bytes, 0, spMc.BytesToRead);
                                    Thread.Sleep(50);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }

                            Thread.Sleep(300);

                            spMc.Write(trama, 0, trama.Length);

                            if (i == 5)
                            {
                                Thread.Sleep(600);

                                respuestaSerial = null;
                                auxiliar = null;

                                while (spMc.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[spMc.BytesToRead];
                                        spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                        Thread.Sleep(50);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        byte[] bytes = new byte[spMc.BytesToRead];
                                        spMc.Read(bytes, 0, spMc.BytesToRead);
                                        Thread.Sleep(50);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }

                                Byte[] ChkCRC = new Byte[17];
                                Array.Copy(respuestaSerial, 10, corte9, 10, 8);
                                Array.Copy(corte9, 1, ChkCRC, 0, 17);
                                Crc16Ccitt crc = new Crc16Ccitt();
                                ushort a = crc.CCITT_CRC16(ChkCRC);
                                Byte[] aCrc = BitConverter.GetBytes(a);
                                Array.Reverse(aCrc);
                                Array.Copy(aCrc, 0, corte9, 18, 2);

                                Array.Copy(lan, 0, corte64, 17, 4);
                                Byte[] ChkCRC1 = new Byte[32];
                                Array.Copy(corte64, 1, ChkCRC1, 0, 32);
                                ushort a1 = crc.CCITT_CRC16(ChkCRC1);
                                Byte[] aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, corte64, 33, 2);

                                if (tarea == 3)
                                {
                                    Array.Copy(reco, 0, corte66, 0, reco.Length);
                                }

                                Array.Copy(lan, 0, corte66, 17, 4);
                                ChkCRC1 = new Byte[32];
                                Array.Copy(corte66, 1, ChkCRC1, 0, 32);
                                a1 = crc.CCITT_CRC16(ChkCRC1);
                                aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, corte66, 33, 2);

                                Array.Copy(lan, 0, corte68, 17, 4);
                                ChkCRC1 = new Byte[32];
                                Array.Copy(corte68, 1, ChkCRC1, 0, 32);
                                a1 = crc.CCITT_CRC16(ChkCRC1);
                                aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, corte68, 33, 2);

                                Array.Copy(lan, 0, corte70, 15, 4);
                                ChkCRC1 = new Byte[19];
                                Array.Copy(corte70, 1, ChkCRC1, 0, 19);
                                a1 = crc.CCITT_CRC16(ChkCRC1);
                                aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, corte70, 20, 2);

                                Array.Copy(lan, 0, corte72, 25, 4);
                                ChkCRC1 = new Byte[29];
                                Array.Copy(corte72, 1, ChkCRC1, 0, 29);
                                a1 = crc.CCITT_CRC16(ChkCRC1);
                                aCrc1 = BitConverter.GetBytes(a1);
                                Array.Reverse(aCrc1);
                                Array.Copy(aCrc1, 0, corte72, 30, 2);
                            }
                            i += 1;
                        }

                        spMc.Close();

                        #region
                        List<Byte[]> tramasCorte = new List<Byte[]>();
                        tramasCorte.Add(corte2);
                        tramasCorte.Add(corte3);
                        tramasCorte.Add(corte4);
                        tramasCorte.Add(corte5);
                        tramasCorte.Add(corte6);
                        tramasCorte.Add(corte7);
                        tramasCorte.Add(corte8);
                        tramasCorte.Add(corte9);
                        tramasCorte.Add(corte10);
                        tramasCorte.Add(corte11);
                        tramasCorte.Add(corte12);
                        tramasCorte.Add(corte13);
                        tramasCorte.Add(corte14);
                        tramasCorte.Add(corte15);
                        tramasCorte.Add(corte16);
                        tramasCorte.Add(corte17);
                        tramasCorte.Add(corte18);
                        tramasCorte.Add(corte19);

                        List<Byte[]> tramasCorte2 = new List<Byte[]>();
                        tramasCorte2.Add(corte20);
                        tramasCorte2.Add(corte21);
                        tramasCorte2.Add(corte22);
                        tramasCorte2.Add(corte23);
                        tramasCorte2.Add(corte24);
                        tramasCorte2.Add(corte25);
                        tramasCorte2.Add(corte26);
                        tramasCorte2.Add(corte27);
                        tramasCorte2.Add(corte28);
                        tramasCorte2.Add(corte29);
                        tramasCorte2.Add(corte30);
                        tramasCorte2.Add(corte31);
                        tramasCorte2.Add(corte32);
                        tramasCorte2.Add(corte33);
                        tramasCorte2.Add(corte34);
                        tramasCorte2.Add(corte35);
                        tramasCorte2.Add(corte36);
                        tramasCorte2.Add(corte37);
                        tramasCorte2.Add(corte38);
                        tramasCorte2.Add(corte39);
                        tramasCorte2.Add(corte40);
                        tramasCorte2.Add(corte41);
                        tramasCorte2.Add(corte42);
                        tramasCorte2.Add(corte43);
                        tramasCorte2.Add(corte44);
                        tramasCorte2.Add(corte45);
                        tramasCorte2.Add(corte46);
                        tramasCorte2.Add(corte47);
                        tramasCorte2.Add(corte48);
                        tramasCorte2.Add(corte49);
                        tramasCorte2.Add(corte50);
                        tramasCorte2.Add(corte51);
                        tramasCorte2.Add(corte52);
                        tramasCorte2.Add(corte53);
                        tramasCorte2.Add(corte54);
                        tramasCorte2.Add(corte55);
                        tramasCorte2.Add(corte56);
                        tramasCorte2.Add(corte57);
                        tramasCorte2.Add(corte58);
                        tramasCorte2.Add(corte59);
                        tramasCorte2.Add(corte60);
                        tramasCorte2.Add(corte61);
                        tramasCorte2.Add(corte62);
                        tramasCorte2.Add(corte63);
                        tramasCorte2.Add(corte64);
                        tramasCorte2.Add(corte65);
                        tramasCorte2.Add(corte66);
                        tramasCorte2.Add(corte67);
                        tramasCorte2.Add(corte68);
                        tramasCorte2.Add(corte69);
                        tramasCorte2.Add(corte70);
                        tramasCorte2.Add(corte71);
                        tramasCorte2.Add(corte72);
                        tramasCorte2.Add(corte73);
                        tramasCorte2.Add(corte74);
                        tramasCorte2.Add(corte75);
                        tramasCorte2.Add(corte76);
                        tramasCorte2.Add(corte77);
                        tramasCorte2.Add(corte78);
                        tramasCorte2.Add(corte79);
                        tramasCorte2.Add(corte80);
                        tramasCorte2.Add(corte81);
                        tramasCorte2.Add(corte82);
                        tramasCorte2.Add(corte83);
                        tramasCorte2.Add(corte84);
                        tramasCorte2.Add(corte85);
                        #endregion

                        i = 0;
                        foreach (Byte[] tramasC in tramasCorte)
                        {

                            spMe.Write(tramasC, 0, tramasC.Length);
                            Thread.Sleep(800);

                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            while (spMe.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[spMe.BytesToRead];
                                    spMe.Read(respuestaSerial, 0, spMe.BytesToRead);
                                    Thread.Sleep(50);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + spMe.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[spMe.BytesToRead];
                                    spMe.Read(bytes, 0, spMe.BytesToRead);
                                    Thread.Sleep(50);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }

                                Thread.Sleep(800);
                                if (i == 14)
                                {
                                    fKey = new Byte[8];
                                    Array.Copy(respuestaSerial, 15, fKey, 0, 8);
                                }

                                if(i== 15)
                                { 
                                    if (fKey != null)
                                    {
                                        spMc.Open();
                                        timer.Start();
                                        MeterPc = false;
                                        timerTimeOut = false;

                                        while (!timerTimeOut)
                                        {
                                            if (spMc.BytesToRead > 0)
                                            {
                                                timer.Stop();
                                                Thread.Sleep(600);
                                                timerTimeOut = true;
                                                MeterPc = true;
                                                spMc.ReadExisting();
                                            }
                                        }

                                        if (MeterPc == true)
                                        {
                                            Byte[] ArrayCRC = new Byte[26];
                                            Byte[] FArray = { 0x06, 0xEE, 0xD0, 0x00, 0x00, 0x00, 0x11, 0x00, 0x02, 0x01, 0x00, 0x02, 0x01, 0x00, 0x08 };
                                            Byte[] ChkCRC = new Byte[23];

                                            Array.Copy(FArray, 1, ChkCRC, 0, 14);
                                            Array.Copy(fKey, 0, ChkCRC, 14, 8);

                                            Crc16Ccitt crc = new Crc16Ccitt();
                                            ushort a = crc.CCITT_CRC16(ChkCRC);
                                            Byte[] aCrc = BitConverter.GetBytes(a);
                                            Array.Reverse(aCrc);
                                            Array.Copy(FArray, 0, ArrayCRC, 0, 15);
                                            Array.Copy(fKey, 0, ArrayCRC, 15, 8);
                                            Array.Copy(aCrc, 0, ArrayCRC, 24, 2);

                                            spMc.Write(ArrayCRC, 0, ArrayCRC.Length);

                                        }

                                        i = 0;
                                        foreach (Byte[] trama in tramasGMe)
                                        {
                                            respuestaSerial = null;
                                            auxiliar = null;

                                            Thread.Sleep(600);
                                            while (spMc.BytesToRead > 0)
                                            {
                                                if (respuestaSerial == null)
                                                {
                                                    respuestaSerial = new byte[spMc.BytesToRead];
                                                    spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                                    Thread.Sleep(5);
                                                }
                                                else
                                                {
                                                    auxiliar = new byte[respuestaSerial.Length];
                                                    auxiliar = respuestaSerial;
                                                    respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                                    byte[] bytes = new byte[spMc.BytesToRead];
                                                    spMc.Read(bytes, 0, spMc.BytesToRead);
                                                    Thread.Sleep(5);
                                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                                }
                                            }

                                            Thread.Sleep(300);

                                            spMc.Write(trama, 0, trama.Length);

                                            if (i == 5)
                                            {
                                                Thread.Sleep(600);

                                                respuestaSerial = null;
                                                auxiliar = null;

                                                while (spMc.BytesToRead > 0)
                                                {
                                                    if (respuestaSerial == null)
                                                    {
                                                        respuestaSerial = new byte[spMc.BytesToRead];
                                                        spMc.Read(respuestaSerial, 0, spMc.BytesToRead);
                                                        Thread.Sleep(5);
                                                    }
                                                    else
                                                    {
                                                        auxiliar = new byte[respuestaSerial.Length];
                                                        auxiliar = respuestaSerial;
                                                        respuestaSerial = new byte[auxiliar.Length + spMc.BytesToRead];
                                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                                        byte[] bytes = new byte[spMc.BytesToRead];
                                                        spMc.Read(bytes, 0, spMc.BytesToRead);
                                                        Thread.Sleep(5);
                                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                                    }
                                                }

                                                Byte[] ChkCRC = new Byte[17];
                                                Array.Copy(respuestaSerial, 10, corte20, 10, 8);
                                                Array.Copy(corte20, 1, ChkCRC, 0, 17);
                                                Crc16Ccitt crc = new Crc16Ccitt();
                                                ushort a = crc.CCITT_CRC16(ChkCRC);
                                                Byte[] aCrc = BitConverter.GetBytes(a);
                                                Array.Reverse(aCrc);
                                                Array.Copy(aCrc, 0, corte20, 18, 2);
                                            }
                                            i += 1;
                                        }
                                    }

                                }
                                #region

                                #endregion
                            }
                            i += 1;
                        }

                        foreach (Byte[] tramasC2 in tramasCorte2)
                        {

                            spMe.Write(tramasC2, 0, tramasC2.Length);
                            Thread.Sleep(800);

                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            while (spMe.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[spMe.BytesToRead];
                                    spMe.Read(respuestaSerial, 0, spMe.BytesToRead);
                                    Thread.Sleep(5);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + spMe.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[spMe.BytesToRead];
                                    spMe.Read(bytes, 0, spMe.BytesToRead);
                                    Thread.Sleep(5);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            Thread.Sleep(800);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    spMc.Close();
                    spMe.Close();
                }
            }
                return respuesta;
        }

        byte[] EntregaLLave(byte[] fKey)
        {
            int[] cadenaE = new int[64];

            BitArray bits = new BitArray(fKey);

            for (int x = 0; x < bits.Count; x++)
            {
                cadenaE[x] = (bits.Get(x) == true) ? 1 : 0;
            }

            GeneraLlave(cadenaE);

            StringBuilder sb = new StringBuilder(cadenaE.Length / 4);

            for (int y = 0; y < cadenaE.Length; y += 4)
            {
                int v = (cadenaE[y] == 1 ? 8 : 0) |
                        (cadenaE[y + 1] == 1 ? 4 : 0) |
                        (cadenaE[y + 2] == 1 ? 2 : 0) |
                        (cadenaE[y + 3] == 1 ? 1 : 0);

                sb.Append(v.ToString("x1")); // Or "X1"
            }

            byte[] respuestaKey = new byte[sb.ToString().Length / 2];
            for (int z = 0; z < sb.ToString().Length; z += 2)
            {
                respuestaKey[z / 2] = Convert.ToByte(sb.ToString().Substring(z, 2), 16);
            }
            return respuestaKey;
        }

        void inicializaPuerto(int velocidad, String portName, SerialPort sp)
        {
            if (velocidad == 0)
            {
                sp.BaudRate = 9600;
            }
            else
            {
                sp.BaudRate = 4800;
            }
        }

        private static void activaTimeOut(object source, ElapsedEventArgs e)
        {
            timerTimeOut = true;
            timer.Stop();
        }

        private static void activaTimeOut2(object source, ElapsedEventArgs e)
        {
            timer2.Stop();
            //spMe.Write(System.Text.Encoding.UTF8.GetString(new Byte[] { 0x06 }));
        }
    }
}
