import { Injectable } from '@angular/core';
import { EmaReportResponse, EmaReportItem } from '../shared/models/emaReport/emaResponse';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmaReportService {
  baseUrl = 'http://localhost:11813/api/';

  constructor(private http: HttpClient) { 
  }

  getInitialEmaReport(symbol: string, start: Date, end: Date) {
    return this.http.get<EmaReportItem[]>(this.baseUrl + "signals?symbol=" + symbol + "&start=" + start.toLocaleString() + "&end=" + end.toLocaleString());
  }

  getInitialEmaReportAny() {
    return this.http.get(this.baseUrl + "signals");
  }

  getTestData(url: string) {
    return this.http.get<number[][]>(url);
  }
}
