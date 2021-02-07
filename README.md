# PMS5003 .NET
Basic C# .NET 5.0 code to read from Plantower PMS5003

## Wiring

The PMS5003 has the following pins:
| Pin  | Description |
| ---  | ----------- |
| PIN1 | VCC Positive power 5V
| PIN2 | GND Negative power
| PIN3 | SET Set pin /TTL level @ 3.3V，high level or suspending is normal working status, while low level is sleeping mode.
| PIN4 | RX Serial port receiving pin/TTL level @ 3.3V
| PIN5 | TX Serial port sending pin/TTL level @ 3.3V
| PIN6 | RESET Module reset signal /TTL level @ 3.3V，low reset
| PIN7/8 | NC

Make sure PIN1, PIN2, PIN4 and PIN5 are connected to your serial interface

## Usage

`PMS5003.exe <port>`
