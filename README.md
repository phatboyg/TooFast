# Too Fast

A sample to measure the performance of HTTP-based RPC and RabbitMQ-based RPC in both comparison and combination. 

There are three approaches compared using `benchmarks/run.sh` with this project.

1. HTTP request (using Apache Bench) to service, which then uses HttpClient to send a request to the back end service.
2. HTTP request (using Apache Bench) to service, which then uses RequestClient via RabbitMQ to the back end service.
3. RequestClient via RabbitMQ (using .NET client) to service, which then uses RequestClient via RabbitMQ to the back end service.

The 3rd option is configured as _Durable_, to ensure reliable order submission.

Use the included `docker-compose.yml` to setup the environment, and the execute `run.sh` from the `benchmarks` folder.
