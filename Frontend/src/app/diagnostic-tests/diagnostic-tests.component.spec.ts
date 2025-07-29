import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DiagnosticTestsComponent } from './diagnostic-tests.component';

describe('DiagnosticTestsComponent', () => {
  let component: DiagnosticTestsComponent;
  let fixture: ComponentFixture<DiagnosticTestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DiagnosticTestsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DiagnosticTestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
