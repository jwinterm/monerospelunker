# MoneroSpelunker
This is a GUI wrapper made with VB .net for a cryptonight cpuminer. 
Rather than enter arguments on the command line, or create a batch file, the user can just open the program and fill the fields.

## How To Use
Open the program, enter the number of threads to use (probably 1-4, maybe 8 or 16 if you have really nice cpu - take the size of your CPU's cache in MB and divide by two to get an idea of how many threads you can run), 
the mining pool (and port) you'd like to use, and *your Monero wallet address*, and then press start mining. 
The miner should stop if you press stop mining or if you exit the program 
(if it crashes you may need to ctrl-alt-del and kill the minerd.exe process using task manager, but I don't think this should occur). 
Be aware that this is alpha quality software, so the paranthetical in the previous sentence may not be accurate.
