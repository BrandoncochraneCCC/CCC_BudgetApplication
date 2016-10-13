
    $(function () {

        $('.edit').hide();

        $('.ins-btn, .edit-btn, .cancel-btn').on("click", function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit, .display').toggle();
        });

        $('.save-btn').on("click", function () {
            var tr = $(this).parents('tr:first');
            var name = tr.find("#in_name").val();
            var emp_Id = tr.find("#in_emp_Id").val();
            var dept = tr.find("#in_dept").val();
            var f_name = tr.find("#in_f_name").val();
            var l_name = tr.find("#in_l_name").val();
            var s_date = tr.find("#in_s_date").val();
            var e_date = tr.find("#in_e_date").val();
            var benefit_plan = tr.find("#in_benefit_plan").val();
            var emp_type = tr.find("#in_type_emp").val();
            var isSuccess = -1;
            
            
            
           var Employee =
           {
               "EmployeeID": emp_Id,
               "FirstName": f_name,
               "LastName": l_name,
               "StartDate": s_date,
               "EndDate": e_date,
               "BenefitPlanID": benefit_plan,
               "TypeID": emp_type,
               "DepartmentID": dept
           };

            $.ajax({
                url: '/Employees/SaveData/',

                data: JSON.stringify(Employee),

                type: 'POST',

                contentType: 'application/json; charset=utf-8',

                dataType: 'json',

                success: function (result) {
                    isSuccess = result;
                },
                error: function (result) {
                    isSuccess = result;
                }

            }).done(function () {
                if (isSuccess == "1") {

                    tr.find('.edit, .display').toggle();
                    location.reload();
                }
                else {
                    alert("Error. Please check the data");
                }

             

            });

        });


    });


