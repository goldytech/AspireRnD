import {env} from 'node:process';
import {NodeSDK} from '@opentelemetry/sdk-node';
import {
  OTLPTraceExporter,
  OTLPTraceExporterOptions,
} from '@opentelemetry/exporter-trace-otlp-grpc';
import {
  OTLPMetricExporter,
  OTLPMetricExporterOptions,
} from '@opentelemetry/exporter-metrics-otlp-grpc';
import {
  OTLPLogExporter,
  OTLPLogExporterOptions,
} from '@opentelemetry/exporter-logs-otlp-grpc';
import {SimpleLogRecordProcessor} from '@opentelemetry/sdk-logs';
import {PeriodicExportingMetricReader} from '@opentelemetry/sdk-metrics';
import {HttpInstrumentation} from '@opentelemetry/instrumentation-http';
import {ExpressInstrumentation} from '@opentelemetry/instrumentation-express';
import {RedisInstrumentation} from '@opentelemetry/instrumentation-redis-4';
import {diag, DiagConsoleLogger, DiagLogLevel} from '@opentelemetry/api';
import {credentials, ChannelCredentials} from '@grpc/grpc-js';

const environment: string = process.env.NODE_ENV || 'development';

// For troubleshooting, set the log level to DiagLogLevel.DEBUG
//diag.setLogger(new DiagConsoleLogger(), environment === 'development' ? DiagLogLevel.INFO : DiagLogLevel.WARN);

const otlpServer: string | undefined = env.OTEL_EXPORTER_OTLP_ENDPOINT;

if (otlpServer) {
  console.log(`OTLP endpoint: ${otlpServer}`);

  const isHttps: boolean = otlpServer.startsWith('https://');
  const collectorOptions: OTLPTraceExporterOptions &
    OTLPMetricExporterOptions &
    OTLPLogExporterOptions = {
    credentials: !isHttps
      ? credentials.createInsecure()
      : credentials.createSsl(),
  };

  const sdk = new NodeSDK({
    traceExporter: new OTLPTraceExporter(collectorOptions),
    metricReader: new PeriodicExportingMetricReader({
      exportIntervalMillis: environment === 'development' ? 5000 : 10000,
      exporter: new OTLPMetricExporter(collectorOptions),
    }),
    logRecordProcessor: new SimpleLogRecordProcessor({
      export(logs: ReadableLogRecord[], resultCallback: (result: ExportResult) => void): void {
      }, shutdown(): Promise<void> {
        return Promise.resolve(undefined);
      },
      exporter: new OTLPLogExporter(collectorOptions)
    }),
    instrumentations: [
      new HttpInstrumentation(),
      new ExpressInstrumentation(),
      new RedisInstrumentation(),
    ],
  });

  sdk.start();
}
