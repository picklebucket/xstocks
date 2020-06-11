import { Injectable } from '@angular/core';
import { EmaReportResponse, EmaReportItem } from '../shared/models/emaReport/emaResponse';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmaReportService {
  baseUrl = 'http://localhost:11813/api/signals/';

  constructor(private http: HttpClient) { 
  }

  get50EmaBullReport(symbol: string, start: Date, end: Date) {
    return this.http.get<EmaReportItem[]>(this.baseUrl + "Get50EMABullReport?symbol=" + symbol + "&start=" + start.toLocaleString() + "&end=" + end.toLocaleString());
  }

  get50EmaBearReport(symbol: string, start: Date, end: Date) {
    return this.http.get<EmaReportItem[]>(this.baseUrl + "Get50EMABearReport?symbol=" + symbol + "&start=" + start.toLocaleString() + "&end=" + end.toLocaleString());
  }

  get100SMABearReport(symbol: string, start: Date, end: Date) {
    return this.http.get<EmaReportItem[]>(this.baseUrl + "Get100SMABearReport?symbol=" + symbol + "&start=" + start.toLocaleString() + "&end=" + end.toLocaleString());
  }

  get100SMABullReport(symbol: string, start: Date, end: Date) {
    return this.http.get<EmaReportItem[]>(this.baseUrl + "Get100SMABullReport?symbol=" + symbol + "&start=" + start.toLocaleString() + "&end=" + end.toLocaleString());
  }
}
