ab -c 20 -T application/json -n 10000 -p order.json http://localhost:5000/order
ab -c 20 -T application/json -n 10000 -p order.json http://localhost:5000/rabbitmqorder
dotnet run -c Release -p ../src/TooFast.Client/TooFast.Client.csproj
