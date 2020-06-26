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

export class IchiReport {
    candles: CandleIchiReport[];
    ichiItems: IchiReportItem[];
}

export class CandleIchiReport {
    open: number;
    close: number;
    low: number;
    high: number;
    date: Date;
    ticks: number;
    signalType: number;
}

export class IchiReportItem {
    date: Date;
    ticks: number;
    tenkanSen: number;
    kijunSen: number;
    chikouSpan: number;
    senkouSpanA: number;
    senkouSpanB: number;
}

export class IchiSignalModel {
    date: Date;
    signal: string;
}