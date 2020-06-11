export class EmaReportResponse {
    data: EmaReportItem[]
}

export class EmaReportItem {
    open: number;
    close: number;
    date: Date;
    ticks: number;
    indicator: number;
    isSignal: boolean;
}