import { TestBed, inject } from "@angular/core/testing";

describe("Registration",
    () => {
        beforeEach(() => {
            TestBed.configureTestingModule({
                providers: [Registration]
            });
        });

        it("should be created",
            inject([Registration],
                (service: Registration) => {
                    expect(service).toBeTruthy();
                }));
    });