import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SensorTemperatureGraphComponent } from './sensor-temperature-graph.component';

describe('SensorTemperatureGraphComponent', () => {
  let component: SensorTemperatureGraphComponent;
  let fixture: ComponentFixture<SensorTemperatureGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SensorTemperatureGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensorTemperatureGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
