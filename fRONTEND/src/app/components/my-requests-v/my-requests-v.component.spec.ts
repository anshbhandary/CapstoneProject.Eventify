import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyRequestsVComponent } from './my-requests-v.component';

describe('MyRequestsVComponent', () => {
  let component: MyRequestsVComponent;
  let fixture: ComponentFixture<MyRequestsVComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyRequestsVComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyRequestsVComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
