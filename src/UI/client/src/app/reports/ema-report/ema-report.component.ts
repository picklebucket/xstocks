import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts/highstock';
import { EmaReportService } from '../ema-report.service';
import { EmaReportItem } from 'src/app/shared/models/emaReport/emaResponse';

@Component({
  selector: 'app-ema-report',
  templateUrl: './ema-report.component.html',
  styleUrls: ['./ema-report.component.scss']
})
export class EmaReportComponent implements OnInit {
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
  
  constructor(private emaService: EmaReportService) { }

  ngOnInit(): void {
  }

  onShowClick() {
    this.buildChart(this.selected, this.bsRangeValue[0], this.bsRangeValue[1]);
  }

  buildChart(symbol: string, start: Date, end: Date) {
    this.emaService.getInitialEmaReport(symbol, start, end).subscribe(r => {
      this.openData = [];
      this.closeData = [];
      this.emaData = [];
      r.forEach(e => {
        this.openData.push([e.ticks * 1000, e.open]);
        this.closeData.push([e.ticks * 1000, e.close]);
        this.emaData.push([e.ticks * 1000, e.ema]);
        if (e.isEma50) {
          this.emaReport.push(e);
        }
      });
      this.chartOptions.title.text = symbol + ' 50 EMA Report';
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

 