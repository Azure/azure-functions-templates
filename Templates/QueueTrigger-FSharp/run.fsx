open System
open System.IO

let inputPath = Environment.GetEnvironmentVariable("input")
let input = File.ReadAllText(inputPath)
let message = sprintf "F# script processed queue message '%s'" input
System.Console.Out.WriteLine(message)