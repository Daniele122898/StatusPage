import {Component, OnDestroy, OnInit} from '@angular/core';
import {StatusConfig} from '../../models/StatusConfig';
import {StatusConfigService} from '../../services/status-config.service';
import {Subject} from 'rxjs';
import {takeUntil} from 'rxjs/operators';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit, OnDestroy {

  public statusConfigs: StatusConfig[];

  private destroy$ = new Subject();

  constructor(
    private configService: StatusConfigService,
  ) { }

  ngOnInit(): void {
    this.getStatusConfigs();

    this.configService.listRefreshObs.pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.getStatusConfigs();
      });
  }

  private getStatusConfigs(): void {
    this.configService.getStatusConfigs().subscribe(
      resp => {
        this.statusConfigs = resp;
      }, err => {
        console.error(err);
      }
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
  }

}
