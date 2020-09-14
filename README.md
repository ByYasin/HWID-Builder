# Hwid Builder - 7 hardware features
- A small C# console application to create a unique hardware hash.
- Requires .NET Framework 4.8!

- Following details are used in order to calculate a id:

___________________________________________________________________________

• BIOS = Win32_BIOS, Manufacturer, SMBIOSBIOSVersion, IdentificationCode.

• CPU  = Win32_Processor, ProcessorId, UniqueId, Name.

• HDD  = Win32_DiskDrive, Model, TotalHeads.

• GPU  = Win32_VideoController, DriverVersion, Name.

• MAC  = Win32_NetworkAdapterConfiguration, MACAddress.

• OS   =  Win32_OperatingSystem, SerialNumber, Name.

• SCSI = Win32_SCSIController, DeviceID, Name.

___________________________________________________________________________

- After build, the following files must be in the same directory otherwise it will not work!

• Figgle.dll

• Figgle.xml

• HWID Builder.exe
