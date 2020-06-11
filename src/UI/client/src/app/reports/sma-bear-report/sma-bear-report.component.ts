import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts/highstock';
import { EmaReportService } from '../ema-report.service';
import { EmaReportItem } from 'src/app/shared/models/emaReport/emaResponse';

@Component({
  selector: 'app-sma-bear-report',
  templateUrl: './sma-bear-report.component.html',
  styleUrls: ['./sma-bear-report.component.scss']
})
export class SmaBearReportComponent implements OnInit {
  bsRangeValue: Date[] = [];
  selected: string = '';
  Highcharts: typeof Highcharts = Highcharts;
  chartOptions: Highcharts.Options = {
    rangeSelector: {
      selected: 4
    },
    title: {
      text: ''
    },
    xAxis: {
      type: 'datetime'
    },
    series: [],
  };
  openData: number[][] = [];
  closeData: number[][] = [];
  emaData: number[][] = [];
  emaReport: EmaReportItem[] = [];
  
  constructor(private emaService: EmaReportService) {
    let today = new Date();
    let start = new Date();
    start.setDate(today.getDate() - 365);
    this.bsRangeValue = [start, today];
   }

  ngOnInit(): void {
  }

  onShowClick() {
    this.buildChart(this.selected, this.bsRangeValue[0], this.bsRangeValue[1]);
  }

  buildChart(symbol: string, start: Date, end: Date) {
    this.emaService.get100SMABearReport(symbol, start, end).subscribe(r => {
      this.openData = [];
      this.closeData = [];
      this.emaData = [];
      this.emaReport = [];
      r.forEach(e => {
        this.openData.push([e.ticks, e.open]);
        this.closeData.push([e.ticks, e.close]);
        this.emaData.push([e.ticks, e.indicator]);
        if (e.isSignal) {
          this.emaReport.push(e);
        }
      });
      this.chartOptions.title.text = symbol + ' 100 SMA Bear Report';
      this.chartOptions.series = [
        {
          type: 'line',
          data: this.openData,
          name: 'Open',
          color: 'blue'
        },
        {
          type: 'line',
          data: this.closeData,
          name: 'Close',
          color: 'red'
        },
        {
          type: 'line',
          data: this.emaData,
          name: 'Ema',
          color: 'green',
          tooltip: {
            valueDecimals: 2
          }
        }
      ];
      this.Highcharts.stockChart('highcharts-container', this.chartOptions);
    });
  }
}
