using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using IUSA.SAGCFE.SagTabCFE;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace LecturaRex
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] w0   = new Byte[] { 0x06 };
        byte[] w1 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        byte[] w2 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x04, 0x00, 0xff, 0x06, 0xce, 0x5a };
        byte[] w3 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x20, 0x20, 0x20, 0x20, 0x20, 0x61, 0x64, 0x6d, 0x69, 0x6e, 0x13, 0x30 };
        byte[] w4 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x33, 0x36, 0x35, 0x33, 0x36, 0x35, 0x33, 0x36, 0x35, 0x33, 0x36, 0x35, 0x33, 0x36, 0x35, 0x33, 0x36, 0x35, 0x33, 0x36, 0xe4, 0x4c };
        byte[] w21 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x05, 0x00, 0x38, 0x00, 0x00, 0x01, 0xc7, 0xd4, 0x21 };//corte
        byte[] w22 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x05, 0x00, 0x38, 0x00, 0x00, 0x02, 0xc6, 0x35,0x1a };//reco
        byte[] w19 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        byte[] w20= new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        byte[] w5 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x04, 0x21, 0xb6 };//energia activa total fase A posicion 10,11,12,13
        byte[] w6 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x0c, 0x00, 0x04, 0x82, 0x13 };//energia reactiva total faseA posicion 10,11,12,13
        byte[] w7 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x24, 0x00, 0x04, 0x7b, 0xd6 };//energia reactiva Q1  fase Aposicion 10,11,12,13
        byte[] w8 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x54, 0x00, 0x04, 0xa3, 0x56 };//energia reactiva Q4 FASE A posicion 10,11,12,13
        byte[] w9 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x30, 0x00, 0x04, 0x8f, 0x30 };//energia recivida activa FASE A posicion 10,11,12,13
        byte[] w10 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x09, 0x00, 0x00, 0x18, 0x00, 0x04, 0x76, 0xf5 };//energia entregada activa fase A posicion 10,11,12,13
        byte[] w11 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x04, 0x00, 0x04, 0x9f, 0xe0 };//voltaje A posicion 10,11,12,13
        byte[] w12 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x0c, 0x00, 0x04, 0xed, 0x0d };//voltaje B posicion 10,11,12,13
        byte[] w13 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x00, 0x00, 0x04, 0xfe, 0x83 };//corriente A posicion 10,11,12,13
        byte[] w14 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x08, 0x00, 0x04, 0x8c, 0x6e };//voltaje B posicion 10,11,12,13
        byte[] w15 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x18, 0x00, 0x04, 0xa9, 0xc0 };//potencia activa posicion 10,11,12,13
        byte[] w16 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x28, 0x00, 0x04, 0xb7, 0x6d };//potencia reactiva posicion 10,11,12,13
        byte[] w17 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x4e, 0x00, 0x02, 0xa5, 0xf0 };//factor de potencia posicion 10,11,12,13
        byte[] w18 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x04, 0x00, 0x00, 0x46, 0x00, 0x02, 0xd7, 0x1d };//frecuencia posicion 10,11,12,13
        byte[] w25 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x08, 0x3f, 0x08, 0x01, 0x00, 0x00, 0x00, 0x00, 0x03, 0x76, 0xc8 }; //bait 11 si es 27 esta abieto si es 02 esta cerrado 



        byte[] T0 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x05, 0xb0, 0x40 }; //SALUDO
        byte[] T1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x20, 0x62, 0x3a }; //NUMERO MEDIDOR
        byte[] T2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x01, 0xc8, 0xa6 }; //TIPO DE LECTURA
        byte[] T3_1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x29, 0x82, 0x0b };// LECTURA normal
        byte[] T3_2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x34, 0xe6, 0xc0 };// LECTURA bidi
        byte[] T4 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 }; //Estado
        byte[] T5 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x09, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x26, 0x23, 0xdb };// Validacion reco
        byte[] T6 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x00, 0x00, 0xf2, 0x88 };//corte
        byte[] T7 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x80, 0x00, 0x3e, 0x04 };//reconexion
        //instantaneos  b4 00 00 00 00 00 08 3f 00 54 00 00 34 00 16 39 27  
        string[] mac = { "846993047C7E", "2CF05D501246", "2CF05D50123B", "64006A6DE24B", "2CF05D4FF3D0", "BCF4D4ADFE6D", "74E543024954", "842AFD18567C", "4CEB42AD101F","4CEB42C3487B","34CFF61085C4","4CEB42AD2988", "4CEB42AD1BD7", "4CEB42AD1114", "4CEB42AD0FD9", "4CEB42C3AEAB", "4CEB42C8B194", "98BD8957555E", "4CEB42C35DB2", "4CEB42C347FE", "D8F883526FE3", "CC3D826CC57D", "4CEB42ADD158", "4CEB42C42F8E", "B49BB4555752", "BC542F8D793A", "E8039AB74666", "4CEB42AD1182","4CEB42AD17DB","4CEB42AD374D","4CEB42C99DF1", "4CEB42AD1975","4CEB42AD1976", "4CEB42C34722", "4CEB42C8EE93", "C8B29B0F256C", "4CEB42C8E74F", "2CF05D4FF3D0", "5CC5D4C24250", "842AFD179F0C", "4CEB42AD100B", "347DF6B3C9EB", "4CEB42C9A784", "4CEB42AD198F","4CEB42ADCA56", "4CEB42ADCA55", "4CEB42C8EE93", "60EB695CADE2", "F07BCBA106B1", "4CEB42AD1A10", "4CEB42AD16D2", "4CEB42AD16FA", "4CEB42C3AE79", "4CEB42C42F26", "4CEB42C42F29", "C8B29B139345", "34CFF6D556CE", "4CEB42ADD13F", "4CEB42C8F0F5", "EC2E98E33E59", "4CEB42AD29B5", "025041000001", "4CEB42C8C5E4", "4CEB42AD16CE", "4CEB42AD1992", "84699304A602", "4CEB42C42F8E", "4CEB42C42FB6", "4CEB42AD384C", "BC542F89C722","34CFF6157115", "4CEB42C3342C", "4CEB42AD36D1", "4CEB42AD36D1", "4CEB42C3038F", "4CEB42C3038F", "4CEB42C9BF1C", "4CEB42C9BF85", "4CEB42C8C51B", "4CEB42C8C5A2", "4CEB42C8C59A", "4CEB42AD0D9F", "4CEB42C9BFCC", "4CEB42C9A162", "60EB697ADEB6", "4CEB42C42FB6","4CEB42C42F8E", "9457A5BB0FAD", "CC3D826CB484", "7CB0C29C9F91", "4CEB42C9A162", "D80F99542B25", "4CEB42C3038E", "4CEB42AD36D0", "4CEB42C3342C", "4CEB42C8C543", "4CEB42C8C540", "4CEB42CA6205","4CEB42CA6206","34CFF6105013", "F07BCBA10D1A", "60EB695CF897","4CEB42C8C543", "4CEB42C3AE4C","BC542F89A323","4CEB42C8EEA2","4CEB42C9172E","4CEB42C33B65","4CEB42C9A171","4CEB42C37F17","34CFF6D556CE","4CEB42C346B4","4CEB42C99C08", "5CC5D4C25529","4CEB42AD5016","4CEB42C300EB","4CEB42C2BAAF", "4CEB42AD0E08","4CEB42AD0E0D","CC3D827774E6","C8B29B139345","4CEB42C9OCC5","4CEB42C8EF01","4CEB42C9170B","4CEB42C8FE28","4CEB42C8EEA2","C8B29B105A95","4CEB42C347A9","4CEB42ADD126","4CEB42ADD14D", "4CEB42ADBE8E", "4CEB42C3AE79", "C8B29B136F0F", "4CEB42C964A3", "4CEB42C9649F", "347DF6BDFD3A", "BC542F89C722", "4CEB42C42F29", "D8F883526FE3", "34CFF6157CAA", "F07BCBA1E1A6", "60EB69695D8A79","4CEB42AD5683", "4CEB42C9054A", "4CEB42AD3211", "4CEB42ADCAA5","4CEB42C34817","4CEB42C34844","4CEB42C42FB1","4CEB42ADBE8E", "4CEB42C347A9", "4CEB42ADD126", "4CEB42AD0D9F", "4CEB42AD1816", "60EB697AAFBD", "4CEB42C3485E", "4CEB42AD3751", "4CEB42AD3784","5CC5D4C2902F","4CEB42AD4FFD","4CEB42AD370C","5CC5D4C24944","4CEB42C3BC4D,","4CEB42C8EF01","4CEB42ADD0B8","4CEB42C8EEA2","4CEB42C8FEfe24","4CEB42C9178D","4CEB42C9177E","4CEB42C8B162","4CEB42C90CC5","5CC5D4C25529","4CEB42C8B22A","4CEB42C346B4","4CEB42C99C08","4CEB42C3D24C","4CEB42AD3251", "BC542F89BFBB", "4CEB42C2EA70", "4CEB42AD1C0D","4CEB42C3046A", "4CEB42AE60B9", "60EB695D8A32", "5CAC4CC96C36", "4CEB42C3342B", "4CEB42AD36D0", "4CEB42AD503E", "4CEB42C3D192", "4CEB42C8C5A8", "4CEB42C9BF2F", "60EB69CE4404", "4CEB42C3AE1F", "4CEB42C3AE24", "4CEB42C3AE47", "4CEB42C30456", "4CEB42C3AE8D", "4CEB42C34731", "4CEB42C44E29", "4CEB42C8F0F5", "347DF6BDEB06", "4CEB42C8C50D", "4CEB42C8C511", "4CEB42C2E22E", "4CEB42C2E22D", "70F395470B76", "60EB695CFD2E", "4CEB42C8EEA2","4CEB42ADD0B8", "4CEB42C8FE24", "4CEB42C8EF01","34CFF6157291", "80C5F25E4E53", "4CEB42AD1B23", "4CEB42C3478", "4CEB42C34782", "4CEB42C8C5F9", "4CEB42C8C5F8", "4CEB42C3AE84", "4CEB42C3AE83", "4CEB42C9172E","4CEB42C9172F", "F07BCBA10B70", "60EB695D01B2", "4CEB42C91782", "4CEB42C9177E", "4CEB42AD2984", "4CEB42AD2983", "4CEB42C8C508","4CEB42C8C509","60EB695CFD2E", "4CEB42C91791", "4CEB42C406A2", "4CEB42AD29BF", "4CEB42AD2987", "4CEB42ADCB0D", "4CEB42AD1811", "4CEB42C8C5FC", "4CEB42C8C5F8", "5CC5D4C25529", "4CEB42C99DED", "F07BCBA0FB4D", "F07BCBA10697", "4CEB42ADCB09", "4CEB42ADCAB9","4CEB42AD180D","4CEB42AD17D6", "F07BCBA0F333", "4CEB42C8FE24","4CEB42C8EF01","4CEB42C8EEA2","4CEB42ADD0B8","4CEB42C3D192", "4CEB42C3038E", "4CEB42C9A1F3", "4CEB42AD5684", "4CEB42C8C53F","4CEB42C9054B", "4CEB42AE73CA", "4CEB42AD17EB", "4CEB42AD2A97", "4CEB42AD1B27", "4CEB42AD17D7", "4CEB42AD3212", "4CEB42AD0DB9", "4CEB42AD1007", "4CEB42AD10D4", "4CEB42AD2970", "4CEB42C8FE02", "4CEB42AD1043", "4CEB42C34728", "4CEB42AD2470", "4CEB42AD1B27", "4CEB42AD17D7", "4CEB42AD3212", "4CEB42AD0DB9", "4CEB42AD17EB", "4CEB42AD1007", "4CEB42AD10D4", "4CEB42AD2970", "4CEB42C8FE02", "4CEB42AD1043", "4CEB42C34728", "4CEB42AD2470", "4CEB42AD1007", "4CEB42AD10D4", "4CEB42AD2970", "4CEB42C8FE02", "4CEB42AD1043", "4CEB42C34728", "4CEB42AD2470", "4CEB42ADBE8E", "4CEB42C44E2E" ,"4CEB42C44E0F", "4CEBC3AE83", "4CEB42C9172E", "78843CB09F3B", "68F72871F222", "782BCBC82405", "F078CBA1E1C7","4CEB42C9C04D", "4CEB42C9BFB8", "4CEB42C9BF9F", "025041000001","4CEB42C37F17", "4CEB42C42F25", "4CEB42CA6205", "4CEB42AD29B5", "34CFF6148A93", "4CEB42C2C2FB","4CEB42AD1B8C","4CEB42C3483F", "4CEB42C8EF15", "4CEB42AD180E", "4CEB42AD29FB", "4CEB42C9A1F4", "4CEB42C9A1F3", "5CC5D4C2902F", "4CEB42AD370C", "4CEB42C3BC4D", "4CEB42C300EB", "4CEB42C99C03", "4CEB42AD1763", "4CEB42C8C5D5", "4CEB42AD29BF", "4CEB42AD36D0", "4CEB42C347A9", "4CEB42C35DD4", "4CEB42AD36D0", "4CEB42C347A9", "4CEB42C34705", "4CEB42C34704", "4CEB42C9A79D", "4CEB42C35D84", "4CEB42AD384C", "4CEB42ADCA8D", "34CFF611BC0F", "4CEB42C347A9", "4CEB42AD36D0", "4CEB42AD5016", "60EB695D004D", "4CEB42C303D9", "4CEB42C8B194", "4CEB42AD1B8C", "4CEB42C34840", "CC3D826CC581","4CEB42AD4FFD", "34CFF60FF032", "34CFF61565DA", "482AE30D0511", "4CEB42AD1763", "4CEB42AD29B5", "4CEB42C9A1F3", "402343262688", "842AFD17A4B2", "4CEB42C99BD6" , "4CEB42C8C5D5", "4CEB42ADCAA5", "4CEB42C34817", "4CEB42C34844", "4CEB42C42FB1" };
    
        String COMPU = "";
       
        byte[] F0 = new Byte[] { 0x06 };
        byte[] F1 = new Byte[] { 0x55 };
        byte[] F2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        byte[] F3 = new Byte[] {  0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x00, 0x80, 0x01, 0x0b, 0x33, 0x19 };
        byte[] F4 = new Byte[] {  0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x59, 0x43, 0x4f, 0x52, 0x54, 0x45, 0x53, 0x20, 0x20, 0x20, 0x20, 0x8a, 0xcb };
        //Byte[] F10 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x05, 0xb1, 0x85 };//captar kh    posiciones 17 y 18 tomar valores literalmente
        Byte[] F13 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x05, 0x32, 0xe6 };//captar kh    posiciones 17 y 18 tomar valores literalmente

        byte[] F5 = new Byte[] {  0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x32, 0x32, 0x32, 0x34, 0x32, 0x36, 0x32, 0x31, 0x32, 0x33, 0x32, 0x35, 0x32, 0x37, 0x30, 0x38, 0x31, 0x39, 0x30, 0x34, 0x46, 0x56 };
        byte[] F6 = new Byte[] {  0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x0d, 0xf9, 0x09 }; // insta
        byte[] F7 = new Byte[] {  0xee, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        byte[] F8 = new Byte[] {  0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        byte[] F9 = new Byte[] {  0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x05, 0x71, 0x4b };//medidor
        byte[] F10 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x0c, 0xf3, 0x7b };//estado relevador
        byte[] F11 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x01, 0x12, 0xba, 0x15 };//corte
        byte[] F12 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x00, 0x13, 0xeb, 0x1d };//reco

        byte[] CCG0 = new Byte[] { 0x06 };
        byte[] CCG1 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x01, 0x21, 0x0b, 0x61 };
        byte[] CCG3 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x56, 0x13 };
        byte[] CCG4 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x33, 0x55, 0x50, 0x25, 0x29, 0x3d, 0x3f, 0x35, 0x37, 0x48, 0x47, 0x32, 0x33, 0x23, 0x54, 0x24, 0x42, 0x5a, 0x39, 0x26, 0x5d, 0x81 };
        byte[] CCG5 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x1C, 0xB2, 0xA5 }; // 

        byte[] CCG6 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        byte[] CCG7 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        byte[] CCG8 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x05, 0x71, 0x4b };//medidor
        byte[] CCG9 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x0c, 0xf3, 0x7b };//estado relevador
        byte[] CCG10 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x01, 0x12, 0xba, 0x15 };//corte
        byte[] CCG11 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x00, 0x13, 0xeb, 0x1d };//reco



        byte[] A0 = new Byte[] { 0x06 };
        byte[] A1 = new Byte[] { 0x55 };
        byte[] A2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        byte[] A3 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x00, 0x80, 0x01, 0x0b, 0x33, 0x19 };
        byte[] A4 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x41, 0x64, 0x6d, 0x69, 0x6e, 0x69, 0x73, 0x74, 0x72, 0x61, 0xf9, 0x04 };
        byte[] A5 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x32, 0x32, 0x32, 0x34, 0x32, 0x36, 0x32, 0x31, 0x32, 0x33, 0x32, 0x35, 0x32, 0x37, 0x30, 0x38, 0x31, 0x39, 0x30, 0x34, 0x46, 0x56 };
        byte[] A6 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x1C, 0xB2, 0xA5 }; // insta
        byte[] A7 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        byte[] A8 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        byte[] A9 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x05, 0x71, 0x4b };//medidor
        byte[] A10 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x0c, 0xf3, 0x7b };//estado relevador
        byte[] A11 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x01, 0x12, 0xba, 0x15 };//corte
        byte[] A12 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x40, 0x00, 0x07, 0x00, 0x04, 0x08, 0x08, 0xdd, 0x00, 0x13, 0xeb, 0x1d };//reco

        byte[] E1despertar = new byte[] { 0x00, 0x10, 0x00, 0x12, 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4f, 0x42 };
        byte[] E1lectura = new byte[] { 0x66, 0x03, 0x00, 0x05, 0x00, 0x08, 0x5c, 0x1a };
        byte[] E1saludo = new byte[] { 0x66, 0x06, 0x00, 0x1c, 0xc3, 0x01, 0xd1, 0x2b };
        byte[] E1reco = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x00, 0x41, 0xde };
        byte[] E1corte = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x01, 0x80, 0x1e };
        byte[] E1med = new byte[] { 0x66, 0x03, 0x00, 0x0e, 0x00, 0x03, 0x67, 0x2f };
        double lectur = 0;

        byte[] E2despertar = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xC3, 0x00, 0x00, 0x01, 0x27, 0x9B };
        byte[] E2despertar2 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x06, 0x00, 0xc4, 0x00, 0x34, 0x70, 0xab };
        byte[] E2despertar3 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x06, 0x00, 0xc5, 0x00, 0x45, 0x94, 0x3a };
        byte[] E2lectura = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x03, 0x00, 0x20, 0x00, 0x5E, 0x30, 0x8C };
      //  byte[] E2saludo = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] E2reco = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xA0, 0xA9 };
        byte[] E2corte = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] E2numero = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xF5, 0x00, 0x03, 0x00, 0x04, 0x00, 0x08, 0x7D, 0x50 };

        byte[] buf = new Byte[6];
        byte[] buf2 = new Byte[6];
        byte[] buf3 = new Byte[6];

        byte[] buf4 = new Byte[6];
        byte[] encrypted0 = null;
        byte[] encrypted1 = null;
        byte[] encrypted2 = null;
        byte[] encrypted3 = null;
        byte[] encrypted4 = null;
        byte[] encrypted5 = null;
        Byte[] arreglo1 = new Byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x30, 0x31 };
        Byte[] arreglo2 = new Byte[] { 0x15, 0x14, 0x13, 0x12, 0x11, 0x10, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00 };

        int ok;
        int tiempo = 300;
        string MARCA = "";
        DateTime Quincena = new DateTime(2024, 08, 19, 0, 0, 0);

        public MainWindow()
        {
            ok = GetMacAddressgood();

            int fecha2 = DateTime.Compare(DateTime.Now, Quincena);
            if (ok == 2 )
            {
                if (fecha2 < 1)
                {
                    InitializeComponent();
                    foreach (String puerto in SerialPort.GetPortNames())
                    {
                        cboPuerto.Items.Add(puerto);
                    }

                }
                else
                {
                    MessageBox.Show("Error: Contacte a Jefe de Laboratorio por caducidad");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Error: Contacte a Jefe de Laboratorio por equipo nuevo");
                this.Close();
            }
        }
        private int GetMacAddressgood()
        {
            // string macAddresses = string.Empty;
            //string macAddresses2 = string.Empty;
            int a = 0;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                int i;
                for (i = 0; i < mac.Length; i++)
                {
                    if (nic.GetPhysicalAddress().ToString() == mac[i])
                    {
                        COMPU = mac[i];
                        a = 2;
                        break;
                    }
                }
                i = 0;
                if (a == 2)
                {
                    break;
                }
            }
            return a;
        }

        private void BtnLectura_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Txt_MED.Text = "";
                Txt_ACT.Text = "";
                Txt_REAC.Text = "";
                Txt_REL.Text = "";
                btnLectura.IsEnabled = false;
                if (MARCA == "rex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] respuestaSerial3 = null;
                    byte[] auxiliar3 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, b = 0, c = 0, d = 0;
                    bool bidi = false;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnLectura.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        serialPort.Write(T2, 0, T2.Length);
                        do { 
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                        } while (respuestaSerial == null);
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                            {
                                b = k;
                                break;
                            }
                        }
                        if (respuestaSerial1[b + 10] == 0x08)
                        {
                            bidi = false;
                        }
                        else 
                        {
                            bidi = true; 
                        }


                        if (bidi == false)
                        {
                            serialPort.Write(T3_1, 0, T3_1.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 11 + j];
                                React[j] = 0x00;
                            }
                        }
                        else if (bidi == true)
                        {
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        }
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            Rel = "Abierto";
                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            Rel = "Cerrado";
                        }
                       
                    }
                    Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                    Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                    Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                    Txt_REL.Text = Rel;
                    btnLectura.IsEnabled = true;
                    Archivo(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text, Txt_REL.Text, MARCA);
                    Archivo2(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text);
                    serialPort.Close();
                    MessageBox.Show("Listo");
                }
                else if (MARCA == "iusa")
                {
                    SerialPort serialPort = new SerialPort();

                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        serialPort.Close();
                    }

                    Autogestion iusaMeter = new Autogestion(cboPuerto.Text, 5000);

                    var GX = iusaMeter.Detecta_Generacion_Medidor(5000, "176CKE");

                    if (GX.ToString() != "")
                    {
                        var lectura = iusaMeter.Consultar_Lectura_Autogestion(GX, false);
                        
                        if (lectura.ToString() == "ERROR")
                        {
                            MessageBox.Show("No hay lectura");
                        }
                        else
                        {
                            int a = (Int32.Parse(lectura) / 1000);
                            Txt_ACT.Text = a.ToString();
                            MessageBox.Show("Listo");
                        }
                       
                    }
                    else
                    {
                        MessageBox.Show("No hay lectura");
                    }

                    btnLectura.IsEnabled = true;
                    Archivo(Txt_id.Text, Txt_ACT.Text, "NA", "NA", MARCA);
                }
                else if (MARCA == "eneri1")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(1000).Wait();

                    Byte[] direccionMed = StringToByteArray(Txt_id.Text);

                    Array.Copy(direccionMed, 0, E1lectura, 0, 1);
                    Array.Copy(E1lectura, 0, buf2, 0, 6);
                    uint send = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc3 = BitConverter.GetBytes(send);
                    Array.Copy(aCrc3, 0, E1lectura, 6, 2);

                    Array.Copy(direccionMed, 0, E1med, 0, 1);
                    Array.Copy(E1med, 0, buf3, 0, 6);
                    uint send4 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc4 = BitConverter.GetBytes(send4);
                    Array.Copy(aCrc4, 0, E1med, 6, 2);

                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    serialPort.Write(E1despertar, 0, E1despertar.Length);
                    Task.Delay(1000).Wait();

                    Array.Copy(direccionMed, 0, E1saludo, 0, 1);
                    Array.Copy(E1saludo, 0, buf3, 0, 6);
                    uint send5 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc5 = BitConverter.GetBytes(send5);
                    Array.Copy(aCrc5, 0, E1saludo, 6, 2);
                    
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E1saludo, 0, E1saludo.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                    }
                    else
                    {
                        intentos = 0;
                        do
                        {
                            serialPort.ReadExisting();
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(E1med, 0, E1med.Length);
                            Task.Delay(1000).Wait();

                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);

                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                        }
                        else
                        {
                            byte[] MED = new byte[6];
                            for (int i = 0; i < 6; i++)
                            {
                                MED[i] = respuestaSerial[i + 3];
                            }
                            intentos = 0;
                            respuestaSerial = null;
                            auxiliar = null;
                            do
                            {
                                serialPort.ReadExisting();
                                serialPort.Write(E1lectura, 0, E1lectura.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial == null);
                            if (respuestaSerial == null)
                            {
                                MessageBox.Show("El medidor no contesta");

                            }
                            else
                            {
                                ///ENERI 1
                                // voltaje = respuestaSerial[7] + respuestaSerial[8] * 0.003921569;
                                //corriente = respuestaSerial[9] + respuestaSerial[10] * 0.003921569;
                                lectur = respuestaSerial[3] * 1024.0 + respuestaSerial[4] * 4.0 + respuestaSerial[5] * (1.0 / 64.0) + ((respuestaSerial[5] & 63) * (1.0 / byte.MaxValue) + respuestaSerial[6] * 1.52590218966964E-05);

                                // Volts.Text = voltaje.ToString();
                                // Amper.Text = corriente.ToString();
                                Txt_ACT.Text = lectur.ToString();
                                Txt_MED.Text = Encoding.ASCII.GetString(MED);
                                MessageBox.Show("Listo");

                                Archivo(Txt_MED.Text, Txt_ACT.Text, "NA", "NA", MARCA);
                                Archivo2(Txt_MED.Text, Txt_ACT.Text, "NA");
                            }

                        }
                    }
                    serialPort.Close();
                    btnLectura.IsEnabled = true;

                }
                else if (MARCA == "eneri2")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(1000).Wait();
                    Byte[] direccionMed = Encoding.ASCII.GetBytes(Txt_id.Text);
                    Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
                    byte[] buf = new Byte[E2despertar2.Length - 2];
                    Array.Copy(E2despertar2, 0, buf, 0, E2despertar2.Length - 2);
                    uint send = calc_crc(buf, buf.Length);
                    Byte[] aCrc = BitConverter.GetBytes(send);
                    Array.Reverse(aCrc, 0, aCrc.Length);
                    Array.Copy(aCrc, 2, E2despertar2, E2despertar2.Length - 2,2);
                    Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
                    byte[] buf1 = new Byte[E2despertar2.Length - 2];
                    Array.Copy(E2despertar2, 0, buf1, 0, E2despertar2.Length - 2);
                    uint send1 = calc_crc(buf1, buf1.Length);
                    Byte[] aCrc1 = BitConverter.GetBytes(send1);
                    Array.Reverse(aCrc1, 0, aCrc1.Length);
                    Array.Copy(aCrc1, 2, E2despertar2, E2despertar2.Length - 2, 2);
                    Array.Copy(direccionMed, 0, E2lectura, 10, 6);
                    byte[] buf2 = new Byte[E2lectura.Length - 2];
                    Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
                    uint send2 = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc2 = BitConverter.GetBytes(send2);
                    Array.Reverse(aCrc2, 0, aCrc2.Length);
                    Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                   serialPort.Write(E2despertar2, 0, E2despertar2.Length);
                    Task.Delay(1000).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E2despertar2, 0, E2despertar2.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnLectura.IsEnabled = true;
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        do
                        {
                            serialPort.ReadExisting();
                            serialPort.Write(E2lectura, 0, E2lectura.Length);
                            Task.Delay(1000).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);
                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                        }
                        else
                        {
                            Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                            Array.Copy(respuestaSerial, 31, energy, 0, 4);
                            Array.Reverse(energy);
                            double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                            //  Byte[] voltaj = new Byte[] { 0x3E, 0xEB };//[2];
                            //  Array.Copy(respuestaSerial, 73, voltaj, 0, 2);
                            //  Byte[] corr = new Byte[] { 0x00, 0xE6 };//[2];
                            //  Array.Copy(respuestaSerial, 73, corr, 0, 2);


                            // double Voltaje = (BitConverter.ToDouble(voltaj, 0)) / Math.Pow(2.0D, 7.0D);
                            //double Corriente = (BitConverter.ToDouble(corr, 0)) / Math.Pow(2.0D, 9.0D);
                            //Volts.Text = Voltaje.ToString();
                            //Amper.Text = Corriente.ToString();
                            Txt_ACT.Text = Energia.ToString();
                            Archivo(Txt_id.Text, Txt_ACT.Text, "NA", "NA", MARCA);

                            MessageBox.Show("Listo");
                        }
                    }
                    serialPort.Close();
                    btnLectura.IsEnabled = true;
                }
                else if (MARCA == "mrex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, c = 0;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnLectura.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        
                      
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        
                    }
                    Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                    Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                    Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                    Txt_REL.Text = Rel;
                    btnLectura.IsEnabled = true;
                    Archivo(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text, Txt_REL.Text, MARCA);
                    Archivo2(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text);
                    serialPort.Close();
                    MessageBox.Show("Listo");
                }
                else if (MARCA == "eneri22")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    String[] varios = null;
                    varios = File.ReadAllLines(Txt_id.Text);
                    string fecha = DateTime.Now.ToString("F");
                    string fecha1 = DateTime.Now.ToString("yyyyMMdd");
                    string filepath = Txt_id.Text.Substring(0, Txt_id.Text.Length - 4);
                    filepath = filepath + "_"+ fecha1+".txt";
                    string texto="";
                    bool result = File.Exists(filepath);
                    if (result == false)
                    {
                        FileStream sa = File.Create(filepath);
                    }

                    foreach (string medidor in varios)
                    {
                        Task.Delay(1000).Wait();
                        string lectura = "";
                        Byte[] direccionMed = Encoding.ASCII.GetBytes(medidor);
                        Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
                        byte[] buf = new Byte[E2despertar2.Length - 2];
                        Array.Copy(E2despertar2, 0, buf, 0, E2despertar2.Length - 2);
                        uint send = calc_crc(buf, buf.Length);
                        Byte[] aCrc = BitConverter.GetBytes(send);
                        Array.Reverse(aCrc, 0, aCrc.Length);
                        Array.Copy(aCrc, 2, E2despertar2, E2despertar2.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
                        byte[] buf1 = new Byte[E2despertar2.Length - 2];
                        Array.Copy(E2despertar2, 0, buf1, 0, E2despertar2.Length - 2);
                        uint send1 = calc_crc(buf1, buf1.Length);
                        Byte[] aCrc1 = BitConverter.GetBytes(send1);
                        Array.Reverse(aCrc1, 0, aCrc1.Length);
                        Array.Copy(aCrc1, 2, E2despertar2, E2despertar2.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2lectura, 10, 6);
                        byte[] buf2 = new Byte[E2lectura.Length - 2];
                        Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
                        uint send2 = calc_crc(buf2, buf2.Length);
                        Byte[] aCrc2 = BitConverter.GetBytes(send2);
                        Array.Reverse(aCrc2, 0, aCrc2.Length);
                        Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
                        int intentos = 0;
                        byte[] respuestaSerial = null;
                        byte[] auxiliar = null;
                        double Energia = 0;
                        // serialPort.Write(E2despertar2, 0, E2despertar2.Length);
                        Task.Delay(1000).Wait();
                        do
                        {
                            serialPort.ReadExisting();
                            serialPort.Write(E2despertar2, 0, E2despertar2.Length);
                            Task.Delay(1000).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);

                        if (respuestaSerial == null)
                        {
                            lectura = "no contesta";
                        }
                        else
                        {
                            intentos = 0;
                            respuestaSerial = null;
                            auxiliar = null;
                            do
                            {
                                serialPort.ReadExisting();
                                serialPort.Write(E2lectura, 0, E2lectura.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial == null);
                            if (respuestaSerial == null)
                            {
                                lectura = "no contesta";
                            }
                            else
                            {
                                Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                                Array.Copy(respuestaSerial, 31, energy, 0, 4);
                                Array.Reverse(energy);
                                Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                                lectura = Energia.ToString();
                            }
                        }
                        
                        string linea0 = medidor+"/" + lectura + "/" + fecha;
                        texto = texto + linea0 + "\r\n";
                    }
                    StreamWriter sw = new StreamWriter(filepath);
                    sw.WriteLine(texto);
                    sw.Close();
                    serialPort.Close();
                    btnLectura.IsEnabled = true;
                }
                else if (MARCA == "eneri11")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(1000).Wait();
                    serialPort.Write(E1despertar, 0, E1despertar.Length);
                    Byte[] direccionMed = StringToByteArray(Txt_id.Text);
                    Array.Copy(direccionMed, 0, E1lectura, 0, 1);

                    Crc16Ccitt crc5 = new Crc16Ccitt();
                    Array.Copy(E1lectura, 0, buf2, 0, 6);
                    uint send = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc3 = BitConverter.GetBytes(send);
                    //Array.Reverse(aCrc3);
                    Array.Copy(aCrc3, 0, E1lectura, 6, 2);

                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    serialPort.Write(E1despertar, 0, E1despertar.Length);
                    Task.Delay(1000).Wait();
                    Array.Copy(direccionMed, 0, E1saludo, 0, 1);
                    Array.Copy(E1saludo, 0, buf3, 0, 6);
                    uint send2 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc4 = BitConverter.GetBytes(send2);
                    Array.Copy(aCrc4, 0, E1saludo, 6, 2);
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E1saludo, 0, E1saludo.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        do
                        {

                            serialPort.ReadExisting();
                            serialPort.Write(E1lectura, 0, E1lectura.Length);
                            Task.Delay(1000).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);
                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");

                        }
                        else
                        {
                           
                            // Volts.Text = voltaje.ToString();
                            // Amper.Text = corriente.ToString();
                            Txt_ACT.Text = lectur.ToString();
                            Txt_MED.Text = Txt_id.Text;
                            MessageBox.Show("Listo");
                        }


                    }
                    Archivo(Txt_MED.Text, Txt_ACT.Text, "NA", "NA", MARCA);

                    serialPort.Close();
                    btnLectura.IsEnabled = true;

                }
                else if (MARCA == "focus")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 9600, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[4];
                    byte[] React = new byte[4];
                    byte[] VoltajeA = new byte[2];
                    byte[] VoltajeB = new byte[2];
                    byte[] VoltajeC = new byte[2];
                    byte[] CorrienteA = new byte[2];
                    byte[] CorrienteB = new byte[2];
                    byte[] CorrienteC = new byte[2];
                    string Rel = "";
                    int a = 0, d = 0;

                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(F2, 0, F2.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F3, 0, F3.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        // SerialPort serialPort = new SerialPort(cboPuerto.Text, 38400, Parity.None, 8, StopBits.One);
                        serialPort.BaudRate = 38400;
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F4, 0, F4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F5, 0, F5.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial1 = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F9, 0, F9.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xee && respuestaSerial1[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial1[d + 9 + j];
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F13, 0, F13.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                           if (respuestaSerial == null)
                           {
                             respuestaSerial = new byte[serialPort.BytesToRead];
                             serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                           }
                           else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                        }
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xee && respuestaSerial1[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            Act[j] = respuestaSerial1[d + 15 + j];
                            React[j] = respuestaSerial1[d + 23 + j];
                        }

                        
                        respuestaSerial = null;
                        auxiliar = null;
                       /* serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F6, 0, F6.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xee && respuestaSerial1[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 2; j++)
                        {
                            VoltajeA[j] = respuestaSerial1[d + 9 + j];
                            VoltajeB[j] = respuestaSerial1[d + 11 + j];


                            byte[] a = new byte[] { 0x1e, 0x09 };
                            string act = BitConverter.ToString(a).Replace("-", "");

                            Byte[] aux = new byte[4];
                            for (int i = 0; i < act.Length; i++)
                            {
                                aux[i] = Convert.ToByte(act.Substring(i, 1), 16);
                            }                                                                                                                                                                               

                            string[] r = new string[4];
                            string aux2 = "";
                            for (int i = 0; i < aux.Length; i++)
                            {

                                r[i] = Convert.ToString(aux[i], 2);
                                if (r[i].Length < 4)
                                {
                                    int l = r[i].Length;
                                    for (int j = 0; j < 4 - l; j++)
                                    {
                                        r[i] = r[i].Insert(0, "0");
                                    }
                                }
                                aux2 += r[i];

                            }

                            byte bu = Convert.ToByte(aux2.Substring(2, 8), 2);
                            byte bus = Convert.ToByte((aux2.Substring(aux2.Length - 6)).Insert(0, "00"), 2);


                        }


                        respuestaSerial = null;
                        auxiliar = null;
                       */
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F7, 0, F7.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                         {
                           if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                           else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                         }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F8, 0, F8.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                          if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                          else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                        }

                        respuestaSerial = null;
                        auxiliar = null;
                    }
                    Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                    Txt_REL.Text = Rel;
                    Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                    Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                    btnCorte.IsEnabled = true;
                    serialPort.Close();
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnLectura.IsEnabled = true;
               
            }
        }
        private void BtnCorte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Txt_MED.Text = "";
                Txt_ACT.Text = "";
                Txt_REAC.Text = "";
                Txt_REL.Text = "";
                btnCorte.IsEnabled = false;
                if (MARCA == "rex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] respuestaSerial3 = null;
                    byte[] auxiliar3 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, b = 0, c = 0, d = 0;
                    bool bidi = false;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        serialPort.Write(T2, 0, T2.Length);
                        do { 
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                    } while (respuestaSerial == null) ;
                    for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                            {
                                b = k;
                                break;
                            }
                        }
                        if (respuestaSerial1[b + 10] == 0x08)
                        {
                            bidi = false;
                        }
                        else 
                        {
                            bidi = true; ;
                        }
                        if (bidi == false)
                        {
                            serialPort.Write(T3_1, 0, T3_1.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 11 + j];
                                React[j] = 0x00;
                            }
                        }
                        else if (bidi == true)
                        {
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        }
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            MessageBox.Show("El relevador ya estaba abierto");
                            Rel = "Abierto";
                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(T6, 0, T6.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T4, 0, T4.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial3[d + 15] == 0x01)
                            {
                                MessageBox.Show("El relevador se abrió");
                                Rel = "Abierto";
                            }
                            else if (respuestaSerial3[d + 15] == 0x00)
                            {
                                MessageBox.Show("Error: NO se realizo corte");
                                Rel = "Cerrado";
                            }
                        }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                        Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                        Txt_REL.Text = Rel;
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    btnCorte.IsEnabled = true;

                }
                else if (MARCA == "iusa")
                {
                    SerialPort serialPort = new SerialPort();

                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        serialPort.Close();
                    }

                    Autogestion iusaMeter = new Autogestion(cboPuerto.Text, 5000);

                    var GX = iusaMeter.Detecta_Generacion_Medidor(5000, "176CKE");

                    if (GX.ToString() != "")
                    {
                        var lectura = iusaMeter.Consultar_Lectura_Autogestion(GX, false);
                        if (lectura.ToString() == "ERROR")
                        {
                            MessageBox.Show("No hay lectura");
                        }
                        else
                        {
                            int a = (Int32.Parse(lectura) / 1000);
                            Txt_ACT.Text = a.ToString();
                            var Cerrar = iusaMeter.Ejecutar_AbrirRelevador(5000);
                            if ((Cerrar.ToString() != "")&&( Cerrar.ToString() != "ERROR"))
                            {
                                Txt_REL.Text = Cerrar.ToString();
                                btnCorte.IsEnabled = true;
                            }
                            else
                            {
                                MessageBox.Show("No hubo corte, intente de nuevo");
                                btnCorte.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay comunicacion");
                        btnCorte.IsEnabled = true;
                    }

                    //var Cerrar = iusaMeter.Ejecutar_CerrarRelevador();



                }
                else if (MARCA == "focus")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 9600, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, d = 0;

                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(F2, 0, F2.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F3, 0, F3.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        // SerialPort serialPort = new SerialPort(cboPuerto.Text, 38400, Parity.None, 8, StopBits.One);
                        serialPort.BaudRate = 38400;
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F4, 0, F4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F5, 0, F5.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial1 = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F9, 0, F9.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xee && respuestaSerial1[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial1[a + 9 + j];
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F10, 0, F10.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                          if (respuestaSerial == null)
                          {
                             respuestaSerial = new byte[serialPort.BytesToRead];
                             serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                          }
                          else
                          {
                             auxiliar = new byte[respuestaSerial.Length];
                             auxiliar = respuestaSerial;
                             byte[] bytes = new byte[serialPort.BytesToRead];
                             respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                             Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                             serialPort.Read(bytes, 0, bytes.Length);
                             Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                          }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                          if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                          {
                          d = k;
                          break;
                          }
                        }
                        if (respuestaSerial[d + 14] == 0x05)
                        {
                          Rel = "Abierto";
                        }
                        else if (respuestaSerial[d + 14] == 0x00)
                        {
                           respuestaSerial = null;
                           auxiliar = null;
                           serialPort.Write(F0, 0, F0.Length);
                           Task.Delay(tiempo).Wait();
                           serialPort.Write(F11, 0, F11.Length);
                           Task.Delay(tiempo).Wait();
                           while (serialPort.BytesToRead > 0)
                           {
                             if (respuestaSerial == null)
                             {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                             }
                             else
                             {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                             }
                           }
                           respuestaSerial = null;
                           auxiliar = null;
                           serialPort.Write(F0, 0, F0.Length);
                           Task.Delay(tiempo).Wait();
                           serialPort.Write(F10, 0, F10.Length);
                           Task.Delay(tiempo).Wait();
                           while (serialPort.BytesToRead > 0)
                           {
                              if (respuestaSerial == null)
                              {
                                 respuestaSerial = new byte[serialPort.BytesToRead];
                                 serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                              }
                              else
                              {
                                 auxiliar = new byte[respuestaSerial.Length];
                                 auxiliar = respuestaSerial;
                                 byte[] bytes = new byte[serialPort.BytesToRead];
                                 respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                 Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                 serialPort.Read(bytes, 0, bytes.Length);
                                 Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                              }
                           }
                           for (int k = 0; k < respuestaSerial.Length; k++)
                           {
                             if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                             {
                                d = k;
                                break;
                             }
                           }
                           if (respuestaSerial[d + 14] == 0x05)
                           {
                              Rel = "Abierto";
                           }
                           else if (respuestaSerial[d + 14] == 0x00)
                           {
                              respuestaSerial = null;
                              auxiliar = null;
                              serialPort.Write(F0, 0, F0.Length);
                              Task.Delay(tiempo).Wait();
                              serialPort.Write(F11, 0, F11.Length);
                              Task.Delay(tiempo).Wait();
                              while (serialPort.BytesToRead > 0)
                              {
                                 if (respuestaSerial == null)
                                 {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                 }
                                 else
                                 {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                 }
                              }
                              respuestaSerial = null;
                              auxiliar = null;
                              serialPort.Write(F0, 0, F0.Length);
                              Task.Delay(tiempo).Wait();
                              serialPort.Write(F10, 0, F10.Length);
                              Task.Delay(tiempo).Wait();
                              while (serialPort.BytesToRead > 0)
                              {
                                   if (respuestaSerial == null)
                                   {
                                      respuestaSerial = new byte[serialPort.BytesToRead];
                                      serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                   }
                                   else
                                   {
                                     auxiliar = new byte[respuestaSerial.Length];
                                     auxiliar = respuestaSerial;
                                     byte[] bytes = new byte[serialPort.BytesToRead];
                                     respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                     Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                     serialPort.Read(bytes, 0, bytes.Length);
                                     Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                   }
                              }
                              for (int k = 0; k < respuestaSerial.Length; k++)
                              {
                                 if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                                 {
                                   d = k;
                                   break;
                                 }
                              }
                              if (respuestaSerial[d + 14] == 0x05)
                              {
                                 Rel = "Abierto";
                              }
                              else if (respuestaSerial[d + 14] == 0x00)
                              {
                                Rel = "Cerrado";
                              }
                           }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F7, 0, F7.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                           if (respuestaSerial == null)
                           {
                              respuestaSerial = new byte[serialPort.BytesToRead];
                              serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                           }
                           else
                           {
                              auxiliar = new byte[respuestaSerial.Length];
                              auxiliar = respuestaSerial;
                              byte[] bytes = new byte[serialPort.BytesToRead];
                              respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                              Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                              serialPort.Read(bytes, 0, bytes.Length);
                              Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                           }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F8, 0, F8.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                          if (respuestaSerial == null)
                          {
                             respuestaSerial = new byte[serialPort.BytesToRead];
                             serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                          }
                          else
                          {
                             auxiliar = new byte[respuestaSerial.Length];
                             auxiliar = respuestaSerial;
                             byte[] bytes = new byte[serialPort.BytesToRead];
                             respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                             Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                             serialPort.Read(bytes, 0, bytes.Length);
                             Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                          }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                    }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_REL.Text = Rel;
                        btnCorte.IsEnabled = true;
                        serialPort.Close();

                    
                }
                else if (MARCA == "eneri1")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    serialPort.Write(E1despertar, 0, E1despertar.Length);
                    Task.Delay(1000).Wait();
                    Byte[] direccionMed = StringToByteArray(Txt_id.Text);

                    Array.Copy(direccionMed, 0, E1saludo, 0, 1);
                    Array.Copy(E1saludo, 0, buf3, 0, 6);
                    uint send2 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc4 = BitConverter.GetBytes(send2);
                    Array.Copy(aCrc4, 0, E1saludo, 6, 2);


                    Array.Copy(direccionMed, 0, E1med, 0, 1);
                    Array.Copy(E1med, 0, buf3, 0, 6);
                    uint send4 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc1 = BitConverter.GetBytes(send4);
                    Array.Copy(aCrc1, 0, E1med, 6, 2);

                    Array.Copy(direccionMed, 0, E1corte, 0, 1);
                    Array.Copy(E1corte, 0, buf2, 0, 6);
                    uint send = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc3 = BitConverter.GetBytes(send);
                    Array.Copy(aCrc3, 0, E1corte, 6, 2);
                    Array.Copy(direccionMed, 0, E1lectura, 0, 1);
                    Array.Copy(E1lectura, 0, buf, 0, 6);
                    uint send3 = calc_crc(buf, buf.Length);
                    Byte[] aCrc = BitConverter.GetBytes(send3);
                    Array.Copy(aCrc, 0, E1lectura, 6, 2);
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;

                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E1saludo, 0, E1saludo.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                    }
                    else
                    {
                        respuestaSerial = null;
                        auxiliar = null;

                        intentos = 0;
                        do
                        {
                            serialPort.ReadExisting();
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(E1med, 0, E1med.Length);
                            Task.Delay(1000).Wait();

                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);

                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                        }
                        else
                        {
                            byte[] MED = new byte[6];
                            for (int i = 0; i < 6; i++)
                            {
                                MED[i] = respuestaSerial[i + 3];
                            }
                            intentos = 0;
                            byte[] respuestaSerial3 = null;
                            byte[] auxiliar3 = null;
                            do
                            {

                                serialPort.ReadExisting();

                                serialPort.Write(E1lectura, 0, E1lectura.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial3 == null);
                            if (respuestaSerial3 == null)
                            {
                                MessageBox.Show("El medidor no contesta");

                            }
                            else
                            {
                                lectur = respuestaSerial3[3] * 1024.0 + respuestaSerial3[4] * 4.0 + respuestaSerial3[5] * (1.0 / 64.0) + ((respuestaSerial3[5] & 63) * (1.0 / byte.MaxValue) + respuestaSerial3[6] * 1.52590218966964E-05);
                                Txt_ACT.Text = lectur.ToString();
                                Txt_MED.Text = Encoding.ASCII.GetString(MED);
                                intentos = 0;

                                respuestaSerial3 = null;
                                auxiliar3 = null;
                                respuestaSerial = null;
                                auxiliar = null;
                                do
                                {
                                    serialPort.ReadExisting();
                                    serialPort.Write(E1corte, 0, E1corte.Length);
                                    Task.Delay(1000).Wait();
                                    while (serialPort.BytesToRead > 0)
                                    {
                                        if (respuestaSerial == null)
                                        {
                                            respuestaSerial = new byte[serialPort.BytesToRead];
                                            serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                        }
                                        else
                                        {
                                            auxiliar = new byte[respuestaSerial.Length];
                                            auxiliar = respuestaSerial;
                                            byte[] bytes = new byte[serialPort.BytesToRead];
                                            respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                            Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                            serialPort.Read(bytes, 0, bytes.Length);
                                            Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                        }
                                    }
                                    intentos += 1;
                                } while (intentos < 3 && respuestaSerial == null);
                                if (respuestaSerial == null)
                                {
                                    MessageBox.Show("El medidor no contesta");
                                }
                                else
                                {
                                    Txt_REL.Text = "Abierto";
                                    MessageBox.Show("Listo");
                                }
                            }

                        }
                        Archivo(Txt_id.Text, Txt_ACT.Text, "NA", Txt_REL.Text, MARCA);
                        Archivo2(Txt_MED.Text, Txt_ACT.Text, "NA");
                    }
                    serialPort.Close();
                    btnCorte.IsEnabled = true;
                }
                else if (MARCA == "eneri2")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(1000).Wait();
                    Byte[] direccionMed = Encoding.ASCII.GetBytes(Txt_id.Text);
                    Array.Copy(direccionMed, 0, E2despertar, 10, 6);
                    byte[] buf = new Byte[E2despertar.Length - 2];
                    Array.Copy(E2despertar, 0, buf, 0, E2despertar.Length - 2);
                    uint send = calc_crc(buf, buf.Length);
                    Byte[] aCrc = BitConverter.GetBytes(send);
                    Array.Reverse(aCrc, 0, aCrc.Length);
                    Array.Copy(aCrc, 2, E2despertar, E2despertar.Length - 2, 2);
                    Array.Copy(direccionMed, 0, E2lectura, 10, 6);
                    byte[] buf2 = new Byte[E2lectura.Length - 2];
                    Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
                    uint send2 = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc2 = BitConverter.GetBytes(send2);
                    Array.Reverse(aCrc2, 0, aCrc2.Length);
                    Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
                    Array.Copy(direccionMed, 0, E2corte, 10, 6);
                    byte[] buf4 = new Byte[E2corte.Length - 2];
                    Array.Copy(E2corte, 0, buf4, 0, E2corte.Length - 2);
                    uint send4 = calc_crc(buf4, buf4.Length);
                    Byte[] aCrc4 = BitConverter.GetBytes(send4);
                    Array.Reverse(aCrc4, 0, aCrc4.Length);
                    Array.Copy(aCrc4, 2, E2corte, E2corte.Length - 2, 2);
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E2despertar, 0, E2despertar.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnLectura.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        do
                        {
                            serialPort.ReadExisting();
                            serialPort.Write(E2lectura, 0, E2lectura.Length);
                            Task.Delay(1000).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);
                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                            serialPort.Close();
                        }
                        else
                        {
                            Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                            Array.Copy(respuestaSerial, 31, energy, 0, 4);
                            //Byte[] voltaj = new Byte[] { 0x3E, 0xEB };//[2];
                            //Array.Copy(E2lectura, 73, voltaj, 0, 2);
                            //Byte[] corr = new Byte[] { 0x00, 0xE6 };//[2];
                            //Array.Copy(E2lectura, 73, corr, 0, 2);Array.Copy(respuestaSerial, 31, energy, 0, 4);
                            Array.Reverse(energy);
                            double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;

                            // double Voltaje = (BitConverter.ToDouble(voltaj, 0)) / Math.Pow(2.0D, 7.0D);
                            //double Corriente = (BitConverter.ToDouble(corr, 0)) / Math.Pow(2.0D, 9.0D);
                            //Volts.Text = Voltaje.ToString();
                            //Amper.Text = Corriente.ToString();
                            Txt_ACT.Text = Energia.ToString();
                            intentos = 0;
                            respuestaSerial = null;
                            auxiliar = null;
                            do
                            {
                                serialPort.ReadExisting();
                                serialPort.Write(E2corte, 0, E2corte.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial == null);
                            if (respuestaSerial == null)
                            {
                                serialPort.Close();
                                MessageBox.Show("El medidor no contesta");
                                btnCorte.IsEnabled = true;
                            }
                            else
                            {
                                serialPort.Close();
                                Txt_REL.Text = "Abierto";
                                MessageBox.Show("Listo");
                                btnCorte.IsEnabled = true;
                            }

                        }
                    }
                }
                else if (MARCA == "mrex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] respuestaSerial3 = null;
                    byte[] auxiliar3 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, c = 0, d = 0;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                       
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            MessageBox.Show("El relevador ya estaba abierto");
                            Rel = "Abierto";
                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(T6, 0, T6.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T4, 0, T4.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial3[d + 15] == 0x01)
                            {
                                MessageBox.Show("El relevador se abrió");
                                Rel = "Abierto";
                            }
                            else if (respuestaSerial3[d + 15] == 0x00)
                            {
                                MessageBox.Show("Error: NO se realizo corte");
                                Rel = "Cerrado";
                            }
                        }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                        Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                        Txt_REL.Text = Rel;
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    btnCorte.IsEnabled = true;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnCorte.IsEnabled = true;

            }
        }
        private void BtnReconexion_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Txt_MED.Text = "";
                Txt_ACT.Text = "";
                Txt_REAC.Text = "";
                Txt_REL.Text = "";
                btnReconexion.IsEnabled = false;
                if(MARCA=="rex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] respuestaSerial3 = null;
                    byte[] auxiliar3 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0, b = 0, c = 0, d = 0;
                    bool bidi = false;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnReconexion.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        serialPort.Write(T2, 0, T2.Length);
                        do { 
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                    } while (respuestaSerial == null) ;
                    for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                            {
                                b = k;
                                break;
                            }
                        }
                        if (respuestaSerial1[b + 10] == 0x08)
                        {
                            bidi = false;
                        }
                        else 
                        {
                            bidi = true; ;
                        }
                        if (bidi == false)
                        {
                            serialPort.Write(T3_1, 0, T3_1.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();

                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 11 + j];
                                React[j] = 0x00;
                            }
                        }
                        else if (bidi == true)
                        {
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        }
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T5, 0, T5.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T7, 0, T7.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T4, 0, T4.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial3[d + 15] == 0x01)
                            {
                                MessageBox.Show("Error:El relevador NO se puede cerrar es muy pronto");
                                Task.Delay(tiempo).Wait();
                                respuestaSerial3 = null;
                                Task.Delay(tiempo).Wait();
                                auxiliar3 = null;
                                Task.Delay(tiempo).Wait();
                                serialPort.Write(T7, 0, T7.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                respuestaSerial3 = null;
                                auxiliar3 = null;
                                serialPort.Write(T4, 0, T4.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial3.Length; k++)
                                {
                                    if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                    {
                                        d = k;
                                        break;
                                    }
                                }
                                if (respuestaSerial3[d + 15] == 0x01)
                                {
                                    MessageBox.Show("Error El relevador esta abierto");
                                    Rel = "Abierto";
                                }
                                else if (respuestaSerial3[d + 15] == 0x00)
                                {
                                    MessageBox.Show("Se reconectó");
                                    Rel = "Cerrado";
                                }
                            }
                            else if (respuestaSerial3[d + 15] == 0x00)
                            {
                                MessageBox.Show("Se reconectó");
                                Rel = "Cerrado";
                            }

                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            MessageBox.Show("El relevador ya esta cerrado");
                            Rel = "Cerrado";
                        }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                        Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                        Txt_REL.Text = Rel;
                        btnReconexion.IsEnabled = true;
                        serialPort.Close();
                    }
                    btnReconexion.IsEnabled = true;
                }
                else if(MARCA=="iusa")
                {
                    SerialPort serialPort = new SerialPort();

                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        serialPort.Close();
                    }

                    Autogestion iusaMeter = new Autogestion(cboPuerto.Text, 5000);

                    var GX = iusaMeter.Detecta_Generacion_Medidor(5000, "176CKE");

                    if (GX.ToString() != "")
                    {
                        var lectura = iusaMeter.Consultar_Lectura_Autogestion(GX, false);
                        if (lectura.ToString() == "ERROR")
                        {
                            MessageBox.Show("No hay lectura");
                        }
                        else
                        {
                            int a = (Int32.Parse(lectura) / 1000);
                            Txt_ACT.Text = a.ToString();
                            var Cerrar = iusaMeter.Ejecutar_CerrarRelevador(5000);
                            //var Cerrar = iusaMeter.Ejecutar_CerrarRelevador();

                            if ((Cerrar.ToString() != "") && (Cerrar.ToString() != "ERROR"))
                            {
                                Txt_REL.Text = Cerrar.ToString();
                                btnReconexion.IsEnabled = true;
                            }
                            else
                            {
                                MessageBox.Show("No hubo reconexion, intente de nuevo");
                                btnReconexion.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay lectura");
                        btnReconexion.IsEnabled = true;
                    }
                   
                }
                else if (MARCA == "focus")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 9600, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial1 = null;
                    byte[] auxiliar1 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int  d = 0;

                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(F2, 0, F2.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F3, 0, F3.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        // SerialPort serialPort = new SerialPort(cboPuerto.Text, 38400, Parity.None, 8, StopBits.One);
                        serialPort.BaudRate = 38400;
                        serialPort.Write(F4, 0, F4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F5, 0, F5.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial1 = null;
                        auxiliar1 = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F9, 0, F9.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial1.Length; k++)
                        {
                            if (respuestaSerial1[k] == 0xee && respuestaSerial1[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial1[d + 9 + j];
                        }
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F10, 0, F10.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial[d + 14] == 0x00)
                        {
                            
                            Rel = "Cerrado";
                        }
                        else if (respuestaSerial[d + 14] == 0x05)
                        {
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(F0, 0, F0.Length);
                            Task.Delay(tiempo).Wait();
                            serialPort.Write(F12, 0, F12.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(F0, 0, F0.Length);
                            Task.Delay(tiempo).Wait();
                            serialPort.Write(F10, 0, F10.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial.Length; k++)
                            {
                                if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial[d + 14] == 0x00)
                            {
                                
                                Rel = "Cerrado";
                            }
                            else if (respuestaSerial[d + 14] == 0x01)
                            {

                                Rel = "Cerrado";
                            }
                            else if (respuestaSerial[d + 14] == 0x05)
                            {
                                respuestaSerial = null;
                                auxiliar = null;
                                serialPort.Write(F0, 0, F0.Length);
                                Task.Delay(tiempo).Wait();
                                serialPort.Write(F12, 0, F12.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                respuestaSerial = null;
                                auxiliar = null;
                                serialPort.Write(F0, 0, F0.Length);
                                Task.Delay(tiempo).Wait();
                                serialPort.Write(F10, 0, F10.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial.Length; k++)
                                {
                                    if (respuestaSerial[k] == 0xee && respuestaSerial[k + 1] == 0x00)
                                    {
                                        d = k;
                                        break;
                                    }
                                }
                                if (respuestaSerial[d + 14] == 0x00)
                                {
                                    
                                    Rel = "Cerrado";
                                }
                                else if (respuestaSerial[d + 14] == 0x05)
                                {
                                    
                                    Rel = "Abierto";
                                }

                                
                            }
                        }
                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F7, 0, F7.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        respuestaSerial = null;

                        serialPort.Write(F0, 0, F0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.Write(F8, 0, F8.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }

                    }


                    Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                    Txt_REL.Text = Rel;
                    btnReconexion.IsEnabled = true;
                    serialPort.Close();

                }
                else if (MARCA == "eneri1")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    serialPort.Write(E1despertar, 0, E1despertar.Length);
                    Task.Delay(1000).Wait();
                    Byte[] direccionMed = StringToByteArray(Txt_id.Text);

                    Array.Copy(direccionMed, 0, E1saludo, 0, 1);
                    Array.Copy(E1saludo, 0, buf3, 0, 6);
                    uint send2 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc4 = BitConverter.GetBytes(send2);
                    Array.Copy(aCrc4, 0, E1saludo, 6, 2);


                    Array.Copy(direccionMed, 0, E1med, 0, 1);
                    Array.Copy(E1med, 0, buf3, 0, 6);
                    uint send4 = calc_crc(buf3, buf3.Length);
                    Byte[] aCrc1 = BitConverter.GetBytes(send4);
                    Array.Copy(aCrc1, 0, E1med, 6, 2);

                    Array.Copy(direccionMed, 0, E1reco, 0, 1);
                    Array.Copy(E1reco, 0, buf2, 0, 6);
                    uint send = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc3 = BitConverter.GetBytes(send);
                    Array.Copy(aCrc3, 0, E1reco, 6, 2);

                    Array.Copy(direccionMed, 0, E1lectura, 0, 1);
                    Array.Copy(E1lectura, 0, buf, 0, 6);
                    uint send3 = calc_crc(buf, buf.Length);
                    Byte[] aCrc = BitConverter.GetBytes(send3);
                    Array.Copy(aCrc, 0, E1lectura, 6, 2);


                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;

                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E1saludo, 0, E1saludo.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                    }
                    else
                    {
                        respuestaSerial = null;
                        auxiliar = null;

                        intentos = 0;
                        do
                        {
                            serialPort.ReadExisting();
                            respuestaSerial = null;
                            auxiliar = null;
                            serialPort.Write(E1med, 0, E1med.Length);
                            Task.Delay(1000).Wait();

                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);

                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                        }
                        else
                        {
                            byte[] MED = new byte[6];
                            for (int i = 0; i < 6; i++)
                            {
                                MED[i] = respuestaSerial[i + 3];
                            }
                            intentos = 0;
                            do
                            {

                                respuestaSerial = null;
                                serialPort.ReadExisting();
                                serialPort.Write(E1lectura, 0, E1lectura.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial == null);
                            if (respuestaSerial == null)
                            {
                                MessageBox.Show("El medidor no contesta");

                            }
                            else
                            {
                                lectur = respuestaSerial[3] * 1024.0 + respuestaSerial[4] * 4.0 + respuestaSerial[5] * (1.0 / 64.0) + ((respuestaSerial[5] & 63) * (1.0 / byte.MaxValue) + respuestaSerial[6] * 1.52590218966964E-05);
                                Txt_ACT.Text = lectur.ToString();
                                Txt_MED.Text = Encoding.ASCII.GetString(MED);
                                intentos = 0;
                                respuestaSerial = null;
                                auxiliar = null;
                                do
                                {
                                    serialPort.ReadExisting();
                                    serialPort.Write(E1reco, 0, E1reco.Length);
                                    Task.Delay(1000).Wait();
                                    while (serialPort.BytesToRead > 0)
                                    {
                                        if (respuestaSerial == null)
                                        {
                                            respuestaSerial = new byte[serialPort.BytesToRead];
                                            serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                        }
                                        else
                                        {
                                            auxiliar = new byte[respuestaSerial.Length];
                                            auxiliar = respuestaSerial;
                                            byte[] bytes = new byte[serialPort.BytesToRead];
                                            respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                            Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                            serialPort.Read(bytes, 0, bytes.Length);
                                            Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                        }
                                    }
                                    intentos += 1;
                                } while (intentos < 3 && respuestaSerial == null);
                                if (respuestaSerial == null)
                                {
                                    MessageBox.Show("El medidor no contesta");
                                }
                                else
                                {

                                    Txt_REL.Text = "Cerrado";
                                    MessageBox.Show("Listo");
                                }
                            }

                        }
                        Archivo(Txt_id.Text, Txt_ACT.Text, "NA", Txt_REL.Text, MARCA);
                        Archivo2(Txt_id.Text, Txt_ACT.Text, "NA");
                    }
                    serialPort.Close();
                    btnReconexion.IsEnabled = true;
                }
                else if (MARCA == "eneri2")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(1000).Wait();
                    Byte[] direccionMed = Encoding.ASCII.GetBytes(Txt_id.Text);
                    Array.Copy(direccionMed, 0, E2despertar, 10, 6);
                    byte[] buf = new Byte[E2despertar.Length - 2];
                    Array.Copy(E2despertar, 0, buf, 0, E2despertar.Length - 2);
                    uint send = calc_crc(buf, buf.Length);
                    Byte[] aCrc = BitConverter.GetBytes(send);
                    Array.Reverse(aCrc, 0, aCrc.Length);
                    Array.Copy(aCrc, 2, E2despertar, E2despertar.Length - 2, 2);
                    Array.Copy(direccionMed, 0, E2lectura, 10, 6);
                    byte[] buf2 = new Byte[E2lectura.Length - 2];
                    Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
                    uint send2 = calc_crc(buf2, buf2.Length);
                    Byte[] aCrc2 = BitConverter.GetBytes(send2);
                    Array.Reverse(aCrc2, 0, aCrc2.Length);
                    Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
                    int intentos = 0;
                    Array.Copy(direccionMed, 0, E2reco, 10, 6);
                    byte[] buf4 = new Byte[E2reco.Length - 2];
                    Array.Copy(E2reco, 0, buf4, 0, E2reco.Length - 2);
                    uint send4 = calc_crc(buf4, buf4.Length);
                    Byte[] aCrc5 = BitConverter.GetBytes(send4);
                    Array.Reverse(aCrc5, 0, aCrc5.Length);
                    Array.Copy(aCrc5, 2, E2reco, E2reco.Length - 2, 2);

                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E2despertar, 0, E2despertar.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);

                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnLectura.IsEnabled = true;
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        do
                        {
                           
                            serialPort.ReadExisting();
                            respuestaSerial = null;
                            serialPort.Write(E2lectura, 0, E2lectura.Length);
                            Task.Delay(1000).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                            intentos += 1;
                        } while (intentos < 3 && respuestaSerial == null);
                        if (respuestaSerial == null)
                        {
                            MessageBox.Show("El medidor no contesta");
                        }
                        else
                        {
                            Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                            
                          //  Byte[] voltaj = new Byte[] { 0x3E, 0xEB };//[2];
                        //    Array.Copy(E2lectura, 73, voltaj, 0, 2);
                            Byte[] corr = new Byte[] { 0x00, 0xE6 };//[2];
                           // Array.Copy(E2lectura, 73, corr, 0, 2);
                            Array.Copy(respuestaSerial, 31, energy, 0, 4);
                            Array.Reverse(energy);
                            double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                            // double Voltaje = (BitConverter.ToDouble(voltaj, 0)) / Math.Pow(2.0D, 7.0D);
                            //double Corriente = (BitConverter.ToDouble(corr, 0)) / Math.Pow(2.0D, 9.0D);
                            //Volts.Text = Voltaje.ToString();
                            //Amper.Text = Corriente.ToString();
                            Txt_ACT.Text = Energia.ToString();
                            intentos = 0;
                            respuestaSerial = null;
                            auxiliar = null;
                            do
                            {
                                serialPort.ReadExisting();
                                respuestaSerial = null;
                                serialPort.Write(E2reco, 0, E2reco.Length);
                                Task.Delay(1000).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                intentos += 1;
                            } while (intentos < 3 && respuestaSerial == null);
                            if (respuestaSerial == null)
                            {
                                MessageBox.Show("El medidor no contesta");
                                btnReconexion.IsEnabled = true;
                            }
                            else
                            {

                                Txt_REL.Text = "Cerrado";
                                MessageBox.Show("Listo");
                                btnReconexion.IsEnabled = true;
                            }

                        }
                    }
                }
                if (MARCA == "mrex")
                {
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;
                    byte[] respuestaSerial2 = null;
                    byte[] auxiliar2 = null;
                    byte[] respuestaSerial3 = null;
                    byte[] auxiliar3 = null;
                    byte[] Medidor = new byte[6];
                    byte[] Act = new byte[3];
                    byte[] React = new byte[3];
                    string Rel = "";
                    int a = 0,  c = 0, d = 0;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnReconexion.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial2 == null)
                                {
                                    respuestaSerial2 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                                }
                                else
                                {
                                    auxiliar2 = new byte[respuestaSerial2.Length];
                                    auxiliar2 = respuestaSerial2;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                                React[j] = respuestaSerial2[c + 20 + j];
                            }
                        
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T5, 0, T5.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T7, 0, T7.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T4, 0, T4.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial3[d + 15] == 0x01)
                            {
                                MessageBox.Show("Error:El relevador NO se puede cerrar es muy pronto");
                                Task.Delay(tiempo).Wait();
                                respuestaSerial3 = null;
                                Task.Delay(tiempo).Wait();
                                auxiliar3 = null;
                                Task.Delay(tiempo).Wait();
                                serialPort.Write(T7, 0, T7.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                respuestaSerial3 = null;
                                auxiliar3 = null;
                                serialPort.Write(T4, 0, T4.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial3.Length; k++)
                                {
                                    if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                    {
                                        d = k;
                                        break;
                                    }
                                }
                                if (respuestaSerial3[d + 15] == 0x01)
                                {
                                    MessageBox.Show("Error El relevador esta abierto");
                                    Rel = "Abierto";
                                }
                                else if (respuestaSerial3[d + 15] == 0x00)
                                {
                                    MessageBox.Show("Se reconectó");
                                    Rel = "Cerrado";
                                }
                            }
                            else if (respuestaSerial3[d + 15] == 0x00)
                            {
                                MessageBox.Show("Se reconectó");
                                Rel = "Cerrado";
                            }

                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            MessageBox.Show("El relevador ya esta cerrado");
                            Rel = "Cerrado";
                        }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                        Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                        Txt_REL.Text = Rel;
                        btnReconexion.IsEnabled = true;
                        serialPort.Close();
                    }
                    btnReconexion.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnReconexion.IsEnabled = true;
            }
        }
        public static byte[] fromStringAsciiToByteArray(string input)
        {
            byte[] bytes = new byte[input.Length];
            char[] values = input.ToCharArray();
            for (int i = 0; i < values.Length; i++)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(values[i]);
                // Convert the decimal value to a byte value
                bytes[i] = (byte)value;
            }
            return bytes;
        }
        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        uint calc_crc(byte[] ptbuf, int num)
        {
            uint crc16 = 0xffff;
            uint temp;
            uint flag;

            for (int i = 0; i < num; i++)
            {
                temp = (uint)ptbuf[i]; // temp has the first byte 
                temp &= 0x00ff; // mask the MSB 
                crc16 = crc16 ^ temp; //crc16 XOR with temp 
                for (uint c = 0; c < 8; c++)
                {
                    flag = crc16 & 0x01; // LSBit di crc16 is mantained
                    crc16 = crc16 >> 1; // Lsbit di crc16 is lost 
                    if (flag != 0)
                        crc16 = crc16 ^ 0x0a001; // crc16 XOR with 0x0a001 
                }
            }
            //crc16 = (crc16 >> 8) | (crc16 << 8); // LSB is exchanged with MSB
            return (crc16);
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        public class Crc16Ccitt
        {
            public ushort CCITT_CRC16(byte[] bytes)
            {
                ushort data;
                ushort crc = 0xFFFF;

                for (int j = 0; j < bytes.Length; j++)
                {
                    crc = (ushort)(crc ^ bytes[j]);
                    for (int i = 0; i < 8; i++)
                    {
                        if ((crc & 0x0001) == 1)
                            crc = (ushort)((crc >> 1) ^ 0x8408);
                        else
                            crc >>= 1;
                    }
                }
                crc = (ushort)~crc;
                data = crc;
                crc = (ushort)((crc << 8) ^ (data >> 8 & 0xFF));
                return crc;
            }

            private byte[] GetBytesFromHexString(string strInput)
            {
                Byte[] bytArOutput = new Byte[] { };
                if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
                {
                    SoapHexBinary hexBinary = null;
                    try
                    {
                        hexBinary = SoapHexBinary.Parse(strInput);
                        if (hexBinary != null)
                        {
                            bytArOutput = hexBinary.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return bytArOutput;
            }
        }
        private void RadioBtn_Iusa_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "iusa";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";

            MessageBox.Show("Ingrese Numero Medidor");
            Lbl_id.Content = "Medidor";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Hidden;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Hidden;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Hidden;
            Lbl_Reac.Visibility = Visibility.Hidden;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Visible;
            Lbl_id.Visibility = Visibility.Visible;


        }
        private void RadioBtn_Rex_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "rex";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Visible;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Visible;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Hidden;
            Lbl_id.Visibility = Visibility.Hidden;
        }
        private void RadioBtn_NK151_Checked(object sender, RoutedEventArgs e)
        {
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = ""; 
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Hidden;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Hidden;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Visible;
            Lbl_id.Visibility = Visibility.Visible;
            var result = MessageBox.Show("La Lectura es Individual?","Seleccion",MessageBoxButton.YesNoCancel);
            if (result.ToString() == "No")
            {

                MessageBox.Show("Ingrese Ruta del Archivo ");
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Documentos de texto (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    Lbl_id.Content = "Ruta";
                    Txt_id.Text = openFileDialog.FileName;
                    btnLectura.Focus();
                    MARCA = "eneri22";
                    btnCorte.Visibility = Visibility.Hidden;
                    btnReconexion.Visibility = Visibility.Hidden;
                    Txt_ACT.Visibility = Visibility.Hidden;
                    Txt_REL.Visibility = Visibility.Hidden;
                    Txt_MED.Visibility = Visibility.Hidden;
                    Lbl_Act.Visibility = Visibility.Hidden;
                    Lbl_Med.Visibility = Visibility.Hidden;
                    Lbl_Rel.Visibility = Visibility.Hidden;

                }
            }
            else if (result.ToString() == "Yes")
            {
                MessageBox.Show("Ingrese Numero Medidor");
                Lbl_id.Content = "Medidor";
                Txt_id.Focus();
                Txt_id.Text = "";
                MARCA = "eneri2";
            }
        }
        private void RadioBtn_G155_Checked(object sender, RoutedEventArgs e)
        {
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Hidden;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Hidden;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Visible;
            Lbl_id.Visibility = Visibility.Visible;
            var result = MessageBox.Show("La Lectura es Individual?", "Seleccion", MessageBoxButton.YesNoCancel);
            if (result.ToString() == "No")
            {

                MessageBox.Show("Ingrese Ruta del Archivo ");
                Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    Lbl_id.Content = "Ruta";
                    Txt_id.Text = openFileDialog.FileName;
                    btnLectura.Focus();
                    MARCA = "eneri11";
                }
            }
            else if (result.ToString() == "Yes")
            {
                MessageBox.Show("Ingrese Numero ID 2 Numeros");
                Lbl_id.Content = "ID";
                Txt_id.Focus();
                Txt_id.Text = "";
                MARCA = "eneri1";
            }
        }
        private void RadioBtn_Focus_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "focus";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Hidden;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Hidden;
            Txt_REAC.Visibility = Visibility.Hidden;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Hidden;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Hidden;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Hidden;
            Lbl_id.Visibility = Visibility.Hidden;
            
        }
        private void RadioBtn_mrex_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "mrex";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Visible;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Visible;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Hidden;
            Lbl_id.Visibility = Visibility.Hidden;
        }
        public void Archivo(string med, string ACT, string REACT, string REL, string MARCA)
        {
            string fecha = DateTime.Now.ToString("F");
            string fecha1 = DateTime.Now.ToString("yyyyMMdd");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = arreglo1;
                aes.IV = arreglo2;
                encrypted0 = Encrypt(fecha1, aes.Key, aes.IV);
                encrypted1 = Encrypt(med, aes.Key, aes.IV);
                encrypted2 = Encrypt(ACT, aes.Key, aes.IV);
                encrypted3 = Encrypt(REACT, aes.Key, aes.IV);
                encrypted4 = Encrypt(REL, aes.Key, aes.IV);
                encrypted5 = Encrypt(MARCA, aes.Key, aes.IV);
            }
            string linea0 = BitConverter.ToString(encrypted0);
            string linea1 = BitConverter.ToString(encrypted1);
            string linea2 = BitConverter.ToString(encrypted2);
            string linea3 = BitConverter.ToString(encrypted3);
            string linea4 = BitConverter.ToString(encrypted4);
            string linea5 = BitConverter.ToString(encrypted5);

            //string filepath = "";
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = filepath + ".\\_" + COMPU;
            string path2 = path + ".\\_" + fecha1;

            DirectoryInfo Folder = Directory.CreateDirectory(path);


            if (!File.Exists(path))
            {

                Folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(linea0);
                    sw.WriteLine(linea1);
                    sw.WriteLine(linea2);
                    sw.WriteLine(linea3);
                    sw.WriteLine(linea4);
                    sw.WriteLine(linea5);
                  //  File.SetAttributes(path, FileAttributes.Hidden);
                }
            }
            else if (File.Exists(path))
            {
              /*  if ((Folder.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    //Add Hidden flag    
                    Folder.Attributes |= FileAttributes.Hidden;
                }
              */
                TextWriter sw = new StreamWriter(path2);
                sw.WriteLine(linea0);
                sw.WriteLine(linea1);
                sw.WriteLine(linea2);
                sw.WriteLine(linea3);
                sw.WriteLine(linea4);
                sw.WriteLine(linea5);
                sw.Close();
            }


        }
        public void Archivo2(string med, string ACT, string REACT)
        {
            string fecha = DateTime.Now.ToString("F");
            string fecha1 = DateTime.Now.ToString("yyyyMMdd");
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = filepath + ".\\_LEC_" + COMPU;
            string path2 = path + ".\\_LEC_" + fecha1;
            string linea0 = med + "," + ACT + "," + REACT + "," + fecha;
            bool result = File.Exists(path2);
            if (result == true)
            {
                TextWriter sw = new StreamWriter(path2, true);
                sw.WriteLine(linea0);
                sw.Close();
            }
            else 
            {
                DirectoryInfo Folder = Directory.CreateDirectory(path);
                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(linea0);
                }
            }
        }
        private void RadioBtn_Gabinete_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "mrex";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
            Txt_id.Text = "";
            btnCorte.Visibility = Visibility.Visible;
            btnLectura.Visibility = Visibility.Visible;
            btnReconexion.Visibility = Visibility.Visible;
            Txt_ACT.Visibility = Visibility.Visible;
            Txt_REAC.Visibility = Visibility.Visible;
            Txt_REL.Visibility = Visibility.Visible;
            Txt_MED.Visibility = Visibility.Visible;
            Lbl_Act.Visibility = Visibility.Visible;
            Lbl_Med.Visibility = Visibility.Visible;
            Lbl_Reac.Visibility = Visibility.Visible;
            Lbl_Rel.Visibility = Visibility.Visible;
            Txt_id.Visibility = Visibility.Hidden;
            Lbl_id.Visibility = Visibility.Hidden;
        }
    }
}
