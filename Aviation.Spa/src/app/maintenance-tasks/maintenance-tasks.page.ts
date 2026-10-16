import { Component, OnInit, inject } from '@angular/core';
import { DxDataGridModule } from 'devextreme-angular';
import { MaintenanceTasksPageStore } from './maintenance-tasks-page.store';

@Component({
  imports: [DxDataGridModule],
  providers: [MaintenanceTasksPageStore],
  selector: 'app-maintenance-tasks-page',
  styleUrl: './maintenance-tasks.page.scss',
  templateUrl: './maintenance-tasks.page.html',
})
export class MaintenanceTasksPage implements OnInit {
  protected readonly store = inject(MaintenanceTasksPageStore);

  ngOnInit(): void {
    this.store.loadData();
  }
}
