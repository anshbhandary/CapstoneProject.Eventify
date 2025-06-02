import { ComponentFixture, TestBed } from '@angular/core/testing';
import { VendorhomeComponent } from './vendor-home.component';


describe('VendorHomeComponent', () => {
  let component: VendorhomeComponent;
  let fixture: ComponentFixture<VendorhomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VendorhomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VendorhomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
