import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorTemperatureComponent } from './sensor-temperature.component';

describe('SensorTemperatureComponent', () => {
  let component: SensorTemperatureComponent;
  let fixture: ComponentFixture<SensorTemperatureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorTemperatureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorTemperatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
