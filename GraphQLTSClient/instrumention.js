"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const node_process_1 = require("node:process");
const sdk_node_1 = require("@opentelemetry/sdk-node");
const exporter_trace_otlp_grpc_1 = require("@opentelemetry/exporter-trace-otlp-grpc");
const exporter_metrics_otlp_grpc_1 = require("@opentelemetry/exporter-metrics-otlp-grpc");
const exporter_logs_otlp_grpc_1 = require("@opentelemetry/exporter-logs-otlp-grpc");
const sdk_logs_1 = require("@opentelemetry/sdk-logs");
const sdk_metrics_1 = require("@opentelemetry/sdk-metrics");
const instrumentation_http_1 = require("@opentelemetry/instrumentation-http");
const instrumentation_express_1 = require("@opentelemetry/instrumentation-express");
const instrumentation_redis_4_1 = require("@opentelemetry/instrumentation-redis-4");
const grpc_js_1 = require("@grpc/grpc-js");
const environment = process.env.NODE_ENV || 'development';
// For troubleshooting, set the log level to DiagLogLevel.DEBUG
//diag.setLogger(new DiagConsoleLogger(), environment === 'development' ? DiagLogLevel.INFO : DiagLogLevel.WARN);
const otlpServer = node_process_1.env.OTEL_EXPORTER_OTLP_ENDPOINT;
if (otlpServer) {
    console.log(`OTLP endpoint: ${otlpServer}`);
    const isHttps = otlpServer.startsWith('https://');
    const collectorOptions = {
        credentials: !isHttps
            ? grpc_js_1.credentials.createInsecure()
            : grpc_js_1.credentials.createSsl(),
    };
    const metricReader = new sdk_metrics_1.PeriodicExportingMetricReader({
        exportIntervalMillis: environment === 'development' ? 5000 : 10000,
        exporter: new exporter_metrics_otlp_grpc_1.OTLPMetricExporter(collectorOptions),
    });
    const sdk = new sdk_node_1.NodeSDK({
        traceExporter: new exporter_trace_otlp_grpc_1.OTLPTraceExporter(collectorOptions),
        metricReader: metricReader,
        logRecordProcessor: new sdk_logs_1.SimpleLogRecordProcessor({
            exporter: new exporter_logs_otlp_grpc_1.OTLPLogExporter(collectorOptions),
        }),
        instrumentations: [
            new instrumentation_http_1.HttpInstrumentation(),
            new instrumentation_express_1.ExpressInstrumentation(),
            new instrumentation_redis_4_1.RedisInstrumentation(),
        ],
    });
    sdk.start();
}
