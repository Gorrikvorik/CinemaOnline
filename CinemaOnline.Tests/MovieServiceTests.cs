using CinemaOnline.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaOnline.Tests
{
    //TODO Рализовать тестирование сервиса
    /*https://github.com/MaxNagibator/domiki/blob/master/Domiki/Data/UnitOfWork.cs
     * Тут посмотреть можно что взять для создание MOQ объектов
     * 1) Правильно добавить зависимости для создание объекта сервиса
     * 2) Сначало протестировать UOW.
         *  2.1) Написать тесты для создания и работы транзакций
         *  2.2) Проверить как работает РоллБек при ошибках
         *  2.3) Проверить что репозитории создаются корректно
     * 3) Проверить EntityBaseRepository - 
         *  3.1) как создаются репозитории с вложенными сущностями
         *  3.2) Написать тесты на все методы базого класса EntityBaseRepository
         *      * 3.2.1) Возможно нужно создать  отдельный класс для его тестирования
     * 4) Написать тесты для сервиса
     *   *  4.1) Проверить как работает UOW в сервисе ( как он создается через DI и обычным путем) 
     *           - есть ли разница в поведении UOW
     *   *  4.2) Изучить как работает цепочка CRUD операций от сервиса к кастомному репозиторию
     */
    public class MovieServiceTests :BaseTest
    {

        private readonly IMovieService _movieService;

        public MovieServiceTests( )
        {
            
           // this._movieService = new MovieService()
        }
    }
}
