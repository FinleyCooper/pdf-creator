# PDF Creator
PDF Creator in C# for Gearset's Software Engineering Internships Assessment.
## To run
Unzip the code or clone from https://github.com/finleycooper/pdf-creator.git  
Run
```
dotnet build
```
and then
```
dotnet run -- <filename>
```
or omit the filename to read from SampleText.txt in the current directory.
## About the code
I don't have much experience programming in C#, but I've applied OOP principles which I know from other languages to the project.  
The code works by parsing the file with the class in the `Parser.cs` file. The parser creates a Content instance which contains lists of ParagraphContent instances for each paragraph in the PDF file. Each ParagraphContent instance stores properties on the indent size, if the paragraph is justified and each text segement of the paragraph. The commands in the text file update the state of the Parser to store and with each text addition we either create a new paragraph if we're not currently in one, or add other text segment to the current paragraph. I could add more verification for whitespace, for example if line ends in a full stop, then the following line will had an extra space at the start of it, looking weird. There are similiar niche issues in the parser like this, but they are complicated to deal with and I'm ommitting dealing with them because of time constraints. The PDF writer simply writes the content to the pdf, paragraph by paragraph until we get 3 pages. We also add a handler to add a small page number at the bottom centre of the page when a new page is created.  
The project was coded in Visual Studio Code.
