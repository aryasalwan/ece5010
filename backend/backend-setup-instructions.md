# Commands used for srtting up backend

There is no need to run this section, this was only needed to setup backend for the first time.

Keeping here for general awareness, and so that this is not forgotten.


- `dotnet new classlib` to create the backend project
- `dotnet add package PDFsharp` to add the library needed. (Not needed if not being developed separately from frontend)


To add the backend to the frontend, refer here:
https://learn.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio-code?pivots=dotnet-8-0

In summary, run below:

- `dotnet sln add backend/backend.csproj`
- `dotnet add ece5010/ece5010.csproj reference backend/backend.csproj`