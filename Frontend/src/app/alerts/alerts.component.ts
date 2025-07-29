import { Component, OnInit } from '@angular/core';
import { AlertService } from '../alert';

@Component({
  selector: 'app-alert',
  template: `
    <div *ngIf="message" class="alert-box">
      {{ message }}
    </div>
  `,
  styleUrls: ['./alerts.component.css']
})
export class AlertComponent implements OnInit {
  message: string | null = null;

  constructor(private alertService: AlertService) {}

  ngOnInit(): void {
    this.alertService.alert$.subscribe(msg => {
      this.message = msg;
      setTimeout(() => this.message = null, 3000); // Auto-hide after 3 seconds
    });
  }
}
