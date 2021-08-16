Feature: Verify holidays
    - Verify the API returned new year’s day for the Netherlands for the last 10 years and at least 5 years in the future
    - Verify that “Hemelvaartsdag” (Ascension Day) is always on a Thursday (try to make this check as specific as possible)
    
    @smoke
    Scenario: Service helthcheck
        Given User sends GET request to HolidaysService service with Year 2021 and Country NL
        Then Status Code is 200
            And Holiday objects count more than 0
            And 1 holiday object has valid schema
        
    @holidays        
    Scenario Outline: Verify new year’s day exists
        Given User sends GET request to HolidaysService service with Year <year> and Country NL
        Then User verifies that holiday with name New Year's Day exists

        Examples:
          | year |
          | 2011 |
          | 2012 |        
          | 2013 |        
          | 2014 |        
          | 2015 |        
          | 2016 |        
          | 2017 |        
          | 2018 |        
          | 2019 |        
          | 2020 |        
          | 2021 |        
          | 2022 |        
          | 2023 |        
          | 2024 |        
          | 2025 |        
          | 2026 |        
       
    @holidays
    Scenario: Verify holiday day of the week
        Given User sends GET request to HolidaysService service with Year 2021 and Country NL
        Then User verifies that holiday with localName Hemelvaartsdag on Thursday
        
    @negative    
    Scenario: [Negative] - Verify new year’s day exists
        Given User sends GET request to HolidaysService service with Year 2021 and Country NL
        Then User verifies that holiday with name New Year's Day Fail exists
        
    @negative    
    Scenario: [Negative] - Verify holiday day of the week
        Given User sends GET request to HolidaysService service with Year 2021 and Country NL
        Then User verifies that holiday with localName Hemelvaartsdag on Monday    
       