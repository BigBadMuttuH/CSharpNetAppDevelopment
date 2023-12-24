﻿Object-Relational Mapping (ORM) — это техника, которая позволяет вам использовать объектно-ориентированные языки
программирования для работы с базами данных. В контексте C#, ORM позволяет вам манипулировать данными в базе данных с
помощью объектов в коде, что означает, что вы можете работать с данными, как если бы они были частью вашей обычной
объектно-ориентированной модели, без необходимости писать сложные SQL-запросы.

Вот ключевые принципы ORM:

Абстракция: ORM предоставляет высокоуровневую абстракцию над реляционной базой данных, позволяя разработчикам работать с
объектами, вместо прямого использования SQL-запросов.

Синхронизация: ORM синхронизирует состояние объектов в программе с их представлением в базе данных, автоматически
обрабатывая создание, чтение, обновление и удаление данных (CRUD операции).

Маппинг: В основе ORM лежит "маппинг", процесс связывания объектов в коде с таблицами и записями в базе данных. Каждый
объект представляет собой строку в таблице, а его атрибуты представляют столбцы.

Запросы: Большинство ORM фреймворков предоставляют способы формулировать запросы к базе данных с помощью языка
программирования вместо SQL. Например, LINQ в C# позволяет писать запросы, которые автоматически преобразуются в SQL.

Кэширование: ORM может предложить кэширование запросов и объектов, что улучшает производительность приложения за счет
снижения количества операций ввода-вывода с базой данных.

Управление транзакциями: ORM обычно предоставляет механизмы для управления транзакциями, что позволяет сохранять
целостность данных при выполнении комплексных операций.

Примеры ORM для C#:

Entity Framework (EF): Один из наиболее популярных и полнофункциональных ORM фреймворков для .NET. EF позволяет
разработчикам работать с базами данных, используя объекты .NET и LINQ.

Dapper: Это более легковесный ORM, который обеспечивает минимальную обертку над SQL-запросами для маппинга результатов в
объекты .NET. Он предназначен для тех, кто хочет ближе к SQL, но с преимуществами маппинга.

NHibernate: Это еще один ORM фреймворк для .NET, портированный с Java Hibernate. Он предоставляет полноценную поддержку
для маппинга, кэширования, запросов и управления транзакциями.

Использование ORM может значительно ускорить разработку и сделать код более чистым и понятным, но важно помнить, что это
также может привести к некоторой потере производительности из-за дополнительной абстракции. Кроме того, в очень сложных
случаях использования SQL может быть более эффективным.