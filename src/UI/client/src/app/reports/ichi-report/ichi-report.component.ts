import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts/highstock';
declare var require: any;
require('highcharts/highcharts-more')(Highcharts);
import { EmaReportService } from '../ema-report.service';
import { CandleIchiReport, IchiSignalModel } from 'src/app/shared/models/emaReport/emaResponse';

@Component({
  selector: 'app-ichi-report',
  templateUrl: './ichi-report.component.html',
  styleUrls: ['./ichi-report.component.scss']
})
export class IchiReportComponent implements OnInit {
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
  candleData: number[][] = [];
  ichiCloud: number[][] = [];
  turningLine: number[][] = [];
  baseLine: number[][] = [];
  laggingLine: number[][] = [];
  signals: IchiSignalModel[] = [];
  
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
    this.emaService.getIchiReport(symbol, start, end).subscribe(r => {
      this.candleData = [];
      this.ichiCloud = [];
      this.baseLine = [];
      this.turningLine = [];
      this.laggingLine = [];
      this.signals = [];

      r.candles.forEach(e => {
        this.candleData.push([e.ticks, e.open, e.high, e.low, e.close]);
        if (e.signalType != 0) {
          let signalItem = new IchiSignalModel();
          signalItem.date = e.date;
          switch (e.signalType) {
            case 1: {
              signalItem.signal = "Bullish Strong";
              break;
            }
            case 2: {
              signalItem.signal = "Bullish Weak";
              break;
            }
            case 3: {
              signalItem.signal = "Bearish Strong";
              break;
            }
          }
          this.signals.push(signalItem);
        }
      });
      r.ichiItems.forEach(e => {
        this.ichiCloud.push([e.ticks, e.senkouSpanA, e.senkouSpanB]);
        this.turningLine.push([e.ticks, e.tenkanSen]);
        this.baseLine.push([e.ticks, e.kijunSen])
        this.laggingLine.push([e.ticks, e.chikouSpan]);
      });
      this.chartOptions.title.text = symbol + ' Ichi Report';
      this.chartOptions.series = [
        {
          type: 'candlestick',
          data: this.candleData,
          name: 'Candle'
        },
        {
          type: 'line',
          data: this.laggingLine,
          name: 'Lagging Line',
          color: 'green'
        },
        {
          type: 'line',
          data: this.baseLine,
          name: 'Base Line',
          color: 'red'
        },
        {
          type: 'line',
          data: this.turningLine,
          name: 'Turning Line',
          color: 'blue'
        },
        {
          name: 'Cloud',
          data: this.ichiCloud,
          type: 'arearange',
          lineWidth: 0,
          linkedTo: ':previous',
          fillOpacity: 0.3,
          zIndex: 0,
          marker: {
              enabled: false
          }
      }];
      this.Highcharts.stockChart('highcharts-container', this.chartOptions);
    });
  }
}
