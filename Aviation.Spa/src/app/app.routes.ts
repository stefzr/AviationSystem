import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'maintenance-tasks',
  },
  {
    path: 'maintenance-tasks',
    loadComponent: () =>
      import('./maintenance-tasks/maintenance-tasks.page').then(
        (page) => page.MaintenanceTasksPage,
      ),
  },
  {
    path: '**',
    redirectTo: 'maintenance-tasks',
  },
];
